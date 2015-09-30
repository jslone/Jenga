using UnityEngine;
using System.Collections;

public enum MouseButton : int
{
    Left = 0,
    Right = 1,
    Middle = 2,
}

public class PlanarControls : Singleton<PlanarControls>
{
    public enum Spaces : int
    {
        World,
        Local,
        Camera,
    };

    static LayerMask clickableLayer;
    LayerMask[] spaceMasks;
    public Spaces CurrentSpace = Spaces.World;
    public PhysicMaterial unheldBlockMaterial;
    public PhysicMaterial heldBlockMaterial;
    public FollowBlocks followControls;
    public float followAcceleration;
    public float distanceSpeed;
    LayerMask CurrentSpaceMask { get { return spaceMasks[(int)CurrentSpace]; } }

    public Rigidbody heldPiece { get; private set; }

    Vector3 PiecePosition { get { return heldPiece.transform.FindChild("holdAnchor").transform.position; } }
    Vector3 CameraPosition { get { return CameraControls.Instance.transform.position; } }


    private Vector3 newPosition = Vector3.zero;

    // Called before start
    void Awake()
    {
        Init(this);
        clickableLayer = LayerMask.GetMask("Blocks");
        spaceMasks = new LayerMask[]
        {
            LayerMask.GetMask("Axes_World"),
            LayerMask.GetMask("Axes_Local"),
            LayerMask.GetMask("Axes_Camera"),
        };
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check for selecting a piece
        if(Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Ray mouseClickRay = Mouse.Instance.Ray;
            RaycastHit hitInfo;

            if(Physics.Raycast(mouseClickRay, out hitInfo, float.PositiveInfinity, clickableLayer))
            {
                heldPiece = hitInfo.collider.GetComponent<Rigidbody>();
                SetPieceHeld(true);
                newPosition = hitInfo.point;
                heldPiece.transform.FindChild("holdAnchor").transform.position = hitInfo.point;
            }
        }

        // check for letting go of a piece
        if(Input.GetMouseButtonUp((int)MouseButton.Left) && heldPiece != null)
        {
            SetPieceHeld(false);
            heldPiece = null;
        }

        // cycle spaces
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // wrap around increment space
            CurrentSpace = (Spaces)(((int)CurrentSpace + 1) % spaceMasks.Length);
        }

        // handle mouse movement
        if (heldPiece != null && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            Vector3 cameraToOldPosition = PiecePosition - CameraPosition;

            Ray oldPositionRay = new Ray(CameraPosition, cameraToOldPosition.normalized);
            Ray newPositionRay = Mouse.Instance.Ray;
            newPositionRay.origin = CameraPosition;

            RaycastHit oldInfo, newInfo;
            if (!Physics.Raycast(oldPositionRay, out oldInfo, float.PositiveInfinity, CurrentSpaceMask, QueryTriggerInteraction.Collide))
            {
                Debug.DrawRay(oldPositionRay.origin, 10 * oldPositionRay.direction, Color.red, 10.0f);
                return;
            }

            if (!Physics.Raycast(newPositionRay, out newInfo, float.PositiveInfinity, CurrentSpaceMask, QueryTriggerInteraction.Collide))
            {
                Debug.DrawRay(newPositionRay.origin, 10 * newPositionRay.direction, Color.green, 10.0f);
                return;
            }

            float scaleFactor = cameraToOldPosition.magnitude / oldInfo.distance;
            float realNewDistance = newInfo.distance * scaleFactor;

            newPosition = newPositionRay.origin + newPositionRay.direction * realNewDistance;
        }

        if (Input.GetAxis("Distance") != 0)
        {
            float deltaScalar = distanceSpeed * Input.GetAxis("Distance") * Time.deltaTime;

            // only one plane should be active at a time
            GameObject currentPlane = GameObject.FindGameObjectWithTag("Plane");

            Vector3 direction;
            if (CurrentSpace == Spaces.Camera)
            {
                direction = CameraControls.Instance.transform.forward;
            }
            else
            {
                direction = Mathf.Sign(Vector3.Dot(currentPlane.transform.forward, CameraControls.Instance.transform.forward)) * currentPlane.transform.forward;
            }


            Vector3 delta = deltaScalar * direction;

            Mouse.Instance.PrepMove(newPosition);

            newPosition += delta;

            Mouse.Instance.ExecMove(newPosition);
        }
    }

    void FixedUpdate()
    {
        if (heldPiece != null)
        {
            Vector3 deltaPosition = newPosition - PiecePosition;
            followControls.CurrentHeldBias += followAcceleration * deltaPosition.magnitude;
            heldPiece.velocity = deltaPosition / Time.fixedDeltaTime;
        }
    }

    void SetPieceHeld(bool held)
    {
        followControls.CurrentHeldBias = 1.0f;
    }
}
