using UnityEngine;
using System.Collections;

public class BlockTransparencyEffect : MonoBehaviour {

    static LayerMask clickableLayer;

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
            Clear();
            heldPiece = null;   
        }

        if(heldPiece != null) {
            Fade();
        }
    }

    void Fade() {
        Renderer curRenderer;
        Color curColor;

        curRenderer = heldPiece.GetComponentInChildren<Renderer>();
        curColor = curRenderer.material.color;

        for (int i = 0; i < curRenderer.materials.Length; i++) {
            curRenderer.materials[i].shader = Shader.Find("Custom/Transparent");
            curRenderer.materials[i].color = new Color(curColor.r, curColor.g, curColor.b, 1.0f);
        }
    }

    // Reset piece shader
    void Clear() {
        Renderer curRenderer;
        Color curColor;

        curRenderer = heldPiece.GetComponentInChildren<Renderer>();
        curColor = curRenderer.material.color;
        for (int i = 0; i < curRenderer.materials.Length; i++) {
            curRenderer.materials[i].shader = Shader.Find("Standard");
            curRenderer.materials[i].color = new Color(curColor.r, curColor.g, curColor.b, 1.0f);
        }
  
    }



}
