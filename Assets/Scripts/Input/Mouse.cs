using UnityEngine;
using System.Collections;

public class MouseInteractable : MonoBehaviour
{
    public virtual void cOnMouseEnter()
    {}

    public virtual void cOnMouseExit()
    {}

    public virtual void cOnMouseStay()
    {}

    public virtual void cOnMouseDown()
    {}

    public virtual void cOnMouseUp()
    {}

    public virtual void cOnMouseHold()
    {}
}

[RequireComponent(typeof(RectTransform))]
public class Mouse : Singleton<Mouse>
{
    RectTransform trans;

    private MouseInteractable lastOver = null;

    void Awake()
    {
        Init(this);
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        trans = GetComponent<RectTransform>();
        trans.position = Input.mousePosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Delta = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Position += Delta;

        RaycastHit hitInfo;
        if (Physics.Raycast(Ray, out hitInfo))
        {
            MouseInteractable hit = hitInfo.collider.GetComponent<MouseInteractable>();
            if(hit != lastOver)
            {
                if(lastOver != null)
                {
                    lastOver.cOnMouseExit();
                }
                if (hit != null)
                {
                    hit.cOnMouseEnter();
                }
                lastOver = hit;
            }
            if(lastOver != null)
            {
                lastOver.cOnMouseStay();
                if (Input.GetMouseButtonDown(0))
                {
                    lastOver.cOnMouseDown();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    lastOver.cOnMouseUp();
                }
                if (Input.GetMouseButton(0))
                {
                    lastOver.cOnMouseHold();
                }
            }
        }
        else
        {
            if (lastOver != null) lastOver.cOnMouseExit();
            lastOver = null;
        }
    }

    private Vector3 worldPosCache;
    private Vector3 screenPosCache;
    public void PrepMove(Vector3 pos)
    {
        worldPosCache = pos;
        screenPosCache = CameraControls.Instance.Camera.WorldToScreenPoint(pos);
    }

    public void ExecMove(Vector3 pos)
    {
        Vector3 newScreenPos = CameraControls.Instance.Camera.WorldToScreenPoint(pos);
        Position += (Vector2)(newScreenPos - screenPosCache);
    }

    public Vector2 Position { 
        get { return trans.position; }
        set { trans.position = value; }
    }

    public Vector2 Delta { get; private set; }

    public Ray Ray
    {
        get { return CameraControls.Instance.Camera.ScreenPointToRay(Position); }
    }
}
