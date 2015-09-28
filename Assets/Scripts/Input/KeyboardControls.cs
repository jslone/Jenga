using UnityEngine;
using System.Collections;

public class KeyboardControls : MonoBehaviour
{

    public PlanarControls grabber;
    private Rigidbody currentBlock { get { return grabber.heldPiece; } }

    public bool enableWASD;
    public bool enableQE;

    void Update()
    {
        if (currentBlock != null)
        {
            float moveHorizontal = Input.GetAxis("kHorizontal");
            float moveVertical = Input.GetAxis("kVertical");
            float moveUp = Input.GetAxis("kUp");
            float rotation = Input.GetAxis("kRotation");

            if (enableQE)
            {
                Vector3 rotate = new Vector3(0, rotation, 0);
                //currentBlock.transform.Rotate(rotate, 100 * Time.deltaTime, Space.World);
                //currentBlock.MoveRotation(currentBlock.rotation * Quaternion.Euler(0, rotation * 100 * Time.deltaTime, 0));
                currentBlock.rotation *= Quaternion.Euler(0, rotation * 100 * Time.deltaTime, 0);
            }

            if (enableWASD)
            {
                Vector3 deltaPosition = new Vector3(
                    moveVertical,
                    moveUp,
                    moveHorizontal);
                deltaPosition *= 0.05f;
                currentBlock.velocity = deltaPosition / Time.deltaTime;
            }

        }
    }
}
