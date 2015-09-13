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
    static LayerMask clickableLayer = LayerMask.GetMask("Blocks");
    Rigidbody heldPiece = null;
    Vector3 offset = Vector3.zero;
    float distance = 0.0f;

    Vector3 PiecePosition { get { return heldPiece.transform.position + offset; } }

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
    }

    void FixedUpdate()
    {
        if(heldPiece != null)
        {
            Ray mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = mousePositionRay.origin + distance * mousePositionRay.direction;

            Vector3 deltaPosition = newPosition - PiecePosition;
            heldPiece.velocity = deltaPosition / Time.fixedDeltaTime;
        }
    }

    void SetPieceHeld(bool held)
    {
        heldPiece.useGravity = !held;
        heldPiece.freezeRotation = held;
    }

}
