using UnityEngine;
using System.Collections;

public enum MouseButton : int
{
    Left = 0,
    Right = 1,
    Middle = 2,
}


public class PlanarControls : MonoBehaviour
{
    static LayerMask clickableLayer;

    Rigidbody heldPiece = null;
    Vector3 offset = Vector3.zero;
    float distance = 0.0f;
    bool snap = true;

    Vector3 PiecePosition { get { return heldPiece.transform.position + offset; } }

    // Called before start
    void Awake()
    {
        clickableLayer = LayerMask.GetMask("Blocks");
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
                distance = hitInfo.distance;
            }
        }

        // check for letting go of a piece
        if(Input.GetMouseButtonUp((int)MouseButton.Left) && heldPiece != null)
        {
            SetPieceHeld(false);
            heldPiece = null;
        }

        // toggle snap
        if(Input.GetKeyDown(KeyCode.Space))
        {
            snap = !snap;
        }
    }

    void FixedUpdate()
    {
        if(heldPiece != null)
        {
            Ray mousePositionRay = CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = mousePositionRay.origin + distance * mousePositionRay.direction;

            Vector3 deltaPosition = newPosition - PiecePosition;
            Vector3 freeVariableFlag = Vector3.one;

            // lock closest 
            if (snap)
            {
                Vector3 snappedDirection = CameraControls.Instance.GetClosestDirection();
                Vector3 snappedAbs = new Vector3(Mathf.Abs(snappedDirection.x), Mathf.Abs(snappedDirection.y), Mathf.Abs(snappedDirection.z));

                freeVariableFlag -= snappedAbs;
            }

            // Zeros out directions which should be fixed
            Vector3 fixedDeltaPosition = Vector3.Scale(deltaPosition, freeVariableFlag);

            heldPiece.velocity = fixedDeltaPosition / Time.fixedDeltaTime;
        }
    }

    void SetPieceHeld(bool held)
    {
        heldPiece.useGravity = !held;
        heldPiece.freezeRotation = held;
    }
}
