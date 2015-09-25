using UnityEngine;
using System.Collections;

public class OnMouseHoldEvent : MonoBehaviour {

    public bool isReset;
    public bool isExit;

    float delay = 1.5f;
    float clickTime = 0.0f;

    private bool isClicked;
    private bool isHover;
    private Color oColor;

	public Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
        oColor = rend.material.color;
        isClicked = false;
        isHover = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isHover && isClicked) {
            float timestamp = Time.time;
            if (timestamp - clickTime > delay) {
                if (isReset) {
                    Application.LoadLevel(0);
                }
                if (isExit) {
                    Application.Quit();
                }
            }

        }      
    }

    void OnMouseExit() {
        rend.material.color = oColor;
        isHover = false;
    }
    
    void OnMouseOver() {
        rend.material.color = Color.yellow;
        isHover = true;
    }

    void OnMouseDown() {
        isClicked = true;
        clickTime = Time.time;
    }

    void OnMouseUp() {
        isClicked = false;
        clickTime = 0.0f;
    }
}
