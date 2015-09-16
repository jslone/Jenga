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
    LayerMask CurrentSpaceMask { get { return spaceMasks[(int)CurrentSpace]; } }

    Rigidbody heldPiece = null;
    Vector3 offset = Vector3.zero;

    Vector3 PiecePosition { get { return heldPiece.transform.position + offset; } }
    Vector3 CameraPosition { get { return CameraControls.Instance.transform.position; } }

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
            Ray mouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(mouseClickRay, out hitInfo, float.PositiveInfinity, clickableLayer))
            {
                heldPiece = hitInfo.collider.GetComponent<Rigidbody>();
                SetPieceHeld(true);

                offset = hitInfo.point - heldPiece.transform.position;
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
    }

    void FixedUpdate()
    {
        if(heldPiece != null)
        {
            Vector3 cameraToOldPosition = PiecePosition - CameraPosition;

            Ray oldPositionRay = new Ray(CameraPosition, cameraToOldPosition.normalized);
            Ray newPositionRay = CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            newPositionRay.origin = CameraPosition;

            RaycastHit oldInfo, newInfo;
            if(!Physics.Raycast(oldPositionRay, out oldInfo, float.PositiveInfinity, CurrentSpaceMask))
            {
                Debug.DrawRay(oldPositionRay.origin, 10 * oldPositionRay.direction, Color.red, 10.0f);
                return;
            }

            if(!Physics.Raycast(newPositionRay, out newInfo, float.PositiveInfinity, CurrentSpaceMask))
            {
                Debug.DrawRay(newPositionRay.origin, 10 * newPositionRay.direction, Color.green, 10.0f);
                return;
            }

            float scaleFactor = cameraToOldPosition.magnitude / oldInfo.distance;
            float realNewDistance = newInfo.distance * scaleFactor;

            Vector3 newPosition = newPositionRay.origin + newPositionRay.direction * realNewDistance;

            Vector3 deltaPosition = newPosition - PiecePosition;
            
            heldPiece.velocity = deltaPosition / Time.fixedDeltaTime;
        }
    }

    void SetPieceHeld(bool held)
    {
        heldPiece.useGravity = !held;
        heldPiece.freezeRotation = held;
        heldPiece.transform.FindChild("whenHeld").gameObject.SetActive(held);
    }
}
