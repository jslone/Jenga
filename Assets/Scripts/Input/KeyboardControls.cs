using UnityEngine;
using System.Collections;

public class KeyboardControls : MonoBehaviour {

    private GameObject currentBlock;
    private LayerMask clickableLayer;

    public bool enableWASD;
    public bool enableQE;


	// Use this for initialization
	void Start () {
        clickableLayer = LayerMask.GetMask("Blocks");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown((int)MouseButton.Left)) {
            Ray mouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(mouseClickRay, out hitInfo, float.PositiveInfinity, clickableLayer)) {
                currentBlock = hitInfo.transform.gameObject;
            }
        }
    }

    void FixedUpdate()
    {
        if (currentBlock != null)
        {
            Rigidbody rb = currentBlock.GetComponent<Rigidbody>();
            float moveHorizontal = Input.GetAxis("kHorizontal");
            float moveVertical = Input.GetAxis("kVertical");
            float moveUp = Input.GetAxis("kUp");
            float rotation = Input.GetAxis("kRotation");

            if(enableQE) {
                Vector3 rotate = new Vector3(0, rotation, 0);
                currentBlock.transform.Rotate(rotate, 100 * Time.fixedDeltaTime, Space.World);
            }

            if(enableWASD) {
                Vector3 deltaPosition = new Vector3(
                    moveVertical,
                    moveUp,
                    moveHorizontal);
                deltaPosition *= 0.05f;
                rb.velocity = deltaPosition / Time.fixedDeltaTime;
            }
           
        }
    }
}
