using UnityEngine;
using System.Collections;

public class Mouse : Singleton<Mouse>, Pointer
{
    RectTransform trans;
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
        PointerManager.Instance.pointers.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        Delta = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Position += Delta;
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

    public Vector2 Position
    {
        get { return trans.position; }
        set { trans.position = value; }
    }

    public Vector2 Delta { get; private set; }

    public Collider CurrentOver
    {
        get
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(Ray, out hitInfo))
            {
                return hitInfo.collider;
            }
            return null;
        }
    }

    public Collider LastOver { get; set; }

    public Ray Ray
    {
        get { return CameraControls.Instance.Camera.ScreenPointToRay(Position); }
    }

    public bool isDown { get { return Input.GetMouseButtonDown((int)MouseButton.Left); } }
    public bool isUp { get { return Input.GetMouseButtonUp((int)MouseButton.Left); } }
    public bool isHeld { get { return Input.GetMouseButton((int)MouseButton.Left); } }
    public bool Active { get { return Input.mousePresent; } }
    public MouseInteractable[] LastClicked { get; set; }
}
