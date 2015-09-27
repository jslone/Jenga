using UnityEngine;
using System.Collections;

public class OnMouseHoldEvent : MouseInteractable
{

    public bool isReset;
    public bool isExit;

    public float delay = 1.5f;

    public Color hoverColor;

    float clickTime = 0.0f;

    private bool isClicked;
    private bool isHover;
    private Color oColor;

    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        oColor = rend.material.color;
        isClicked = false;
        isHover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHover && isClicked)
        {
            float timestamp = Time.time;
            float heldDuration = timestamp - clickTime;
            rend.material.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, heldDuration / delay));
            DynamicGI.UpdateMaterials(rend);
            if (heldDuration > delay)
            {
                if (isReset)
                {
                    Application.LoadLevel(0);
                }
                if (isExit)
                {
                    Application.Quit();
                }
            }

        }
    }

    public override void cOnMouseExit()
    {
        rend.material.color = oColor;
        isHover = false;
    }

    public override void cOnMouseEnter()
    {
        rend.material.color = hoverColor;
        isHover = true;
    }

    public override void cOnMouseDown()
    {
        isClicked = true;
        clickTime = Time.time;
    }

    public override void cOnMouseUp()
    {
        isClicked = false;
        clickTime = 0.0f;
        rend.material.SetColor("_EmissionColor", Color.black);
        DynamicGI.UpdateMaterials(rend);
    }
}
