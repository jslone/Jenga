using UnityEngine;
using System.Collections;
using System.Linq;

public class Mouse : Singleton<Mouse>, Pointer
{
    public PlanarControls planarControls;
    public float maxDistance = 10.0f;
    public float distanceSpeed = 2.0f;

    void Awake()
    {
        Init(this);
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        SetWorldFromScreen(Input.mousePosition);
        PointerManager.Instance.pointers.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 newScreenPos = ScreenPosition + delta;
        bool mouseMoved = delta.sqrMagnitude > 0;

        // Move along locked positions
        if((isDown || isHeld) && LastClicked != null && LastClicked.Where(i => i is DragableBlock).Count() > 0)
        {
            Vector3 oldPosition = transform.position;
            if(mouseMoved)
            {
                Vector3 cameraToOldPosition = transform.position - Camera.transform.position;

                Ray oldPositionRay = new Ray(Camera.transform.position, cameraToOldPosition.normalized);

                Ray newPositionRay = CameraControls.Instance.Camera.ScreenPointToRay(newScreenPos);
                newPositionRay.origin = CameraControls.Instance.Camera.transform.position;

                newPositionRay.origin = Camera.transform.position;

                RaycastHit oldInfo, newInfo;
                if (!Physics.Raycast(oldPositionRay, out oldInfo, float.PositiveInfinity, planarControls.CurrentSpaceMask, QueryTriggerInteraction.Collide))
                {
                    Debug.DrawRay(oldPositionRay.origin, 10 * oldPositionRay.direction, Color.red, 10.0f);
                    return;
                }

                if (!Physics.Raycast(newPositionRay, out newInfo, float.PositiveInfinity, planarControls.CurrentSpaceMask, QueryTriggerInteraction.Collide))
                {
                    Debug.DrawRay(newPositionRay.origin, 10 * newPositionRay.direction, Color.green, 10.0f);
                    return;
                }

                float scaleFactor = cameraToOldPosition.magnitude / oldInfo.distance;
                float realNewDistance = newInfo.distance * scaleFactor;

                transform.position = newPositionRay.origin + newPositionRay.direction * realNewDistance;
            }

            if (Input.GetAxis("Distance") != 0)
            {
                float deltaScalar = distanceSpeed * Input.GetAxis("Distance") * Time.deltaTime;

                // only one plane should be active at a time
                GameObject currentPlane = GameObject.FindGameObjectWithTag("Plane");

                Vector3 direction;
                if (planarControls.CurrentSpace == PlanarControls.Spaces.Camera)
                {
                    direction = CameraControls.Instance.transform.forward;
                }
                else
                {
                    direction = Mathf.Sign(Vector3.Dot(currentPlane.transform.forward, CameraControls.Instance.transform.forward)) * currentPlane.transform.forward;
                }

                transform.position += deltaScalar * direction;
            }
        }
        // Move along raycast
        else if(mouseMoved)
        {
            SetWorldFromScreen(newScreenPos);
        }
    }

    void SetWorldFromScreen(Vector2 screenPosition)
    {
        Ray newRay = CameraControls.Instance.Camera.ScreenPointToRay(screenPosition);
        newRay.origin = CameraControls.Instance.Camera.transform.position;

        RaycastHit hitInfo;
        if (Physics.Raycast(newRay, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = newRay.origin + maxDistance * newRay.direction;
        }
    }

    public Vector2 ScreenPosition
    {
        get { return CameraControls.Instance.Camera.WorldToScreenPoint(transform.position); }
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
        get
        {
            Vector3 origin = CameraControls.Instance.Camera.transform.position;
            Vector3 dir = transform.position - origin;
            return new Ray(origin, dir);
        }
    }

    public bool isDown { get { return Input.GetMouseButtonDown((int)MouseButton.Left); } }
    public bool isUp { get { return Input.GetMouseButtonUp((int)MouseButton.Left); } }
    public bool isHeld { get { return Input.GetMouseButton((int)MouseButton.Left); } }
    public bool Active { get { return Input.mousePresent; } }
    public MouseInteractable[] LastClicked { get; set; }
    public Vector3 WorldPosition { get { return transform.position; } }

    private Camera Camera { get { return CameraControls.Instance.Camera; } }
}
