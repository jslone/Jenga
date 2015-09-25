using UnityEngine;
using System.Collections;

public class BlockHoverGlow : MonoBehaviour {

    public Renderer rend; 
    private Color curColor;

	// Use this for initialization
	void Start () {
        curColor = rend.material.color;     
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver() {
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        }
    }

    void OnMouseExit() {
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = new Color(curColor.r, curColor.g, curColor.b, 1.0f);
        }
    }
}
