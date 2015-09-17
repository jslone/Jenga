using UnityEngine;
using System.Collections;

public class BlockTransparencyEffect : MonoBehaviour {

    static LayerMask clickableLayer;
    public float distance = 1f;

    // currently held piece
    // used by blocks to determine fade effect
    Rigidbody heldPiece = null;

    // Use this for initialization
    void Start () {
        clickableLayer = LayerMask.GetMask("Blocks");
    }
	
	// Update is called once per frame
	void Update () {
        // check for selecting a piece
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Ray mouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(mouseClickRay, out hitInfo, float.PositiveInfinity, clickableLayer))
            {
                heldPiece = hitInfo.collider.GetComponent<Rigidbody>();
            }
        }

        // check for letting go of a piece
        if (Input.GetMouseButtonUp((int)MouseButton.Left) && heldPiece != null)
        {
            heldPiece = null;
        }

        if(heldPiece != null) {
            FadeClosest();
        }
    }

    void FadeClosest() {
        GameObject[] tower = GameObject.FindGameObjectsWithTag("Block");
        Vector3 pos = heldPiece.transform.position;
        Rigidbody rb;
        Renderer curRenderer;
        Color curColor;
        float curDistance = 0;

        foreach(GameObject block in tower) {
            rb = block.GetComponent<Rigidbody>();

            curDistance = Vector3.Distance(pos, rb.transform.position);
            curRenderer = block.GetComponentInChildren<Renderer>();
            curColor = curRenderer.material.color;
            for(int i = 0; i < curRenderer.materials.Length; i++) {
                 if (rb != heldPiece && curDistance < distance) {
                     curRenderer.materials[i].shader = Shader.Find("Transparent/Diffuse");
                     curRenderer.materials[i].color = new Color(curColor.r, curColor.g, curColor.b, 0.2f);
                 } else {
                     curRenderer.materials[i].shader = Shader.Find("Standard");
                     curRenderer.materials[i].color = new Color(curColor.r, curColor.g, curColor.b, 1.0f);
                 }
            }    
        }
    }



}
