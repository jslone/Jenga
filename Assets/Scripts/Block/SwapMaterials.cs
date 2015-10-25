using UnityEngine;
using System.Collections;

public class SwapMaterials : MouseInteractable
{
    public Renderer normalObj;
    public Renderer hoveredObj;
    public Renderer clickedObj;

    private GameObject currentObj;

    private bool clicked = false;
    private bool hovered = false;

    void Start()
    {
    }

    public override void cOnMouseDown()
    {
        normalObj.enabled = false;
        hoveredObj.enabled = false;
        clickedObj.enabled = true;
        clicked = true;
    }

    public override void cOnMouseUp()
    {
        clickedObj.enabled = false;
        hoveredObj.enabled = hovered;
        normalObj.enabled = !hovered;
        clicked = false;
    }

    public override void cOnMouseEnter()
    {
        normalObj.enabled = false;
        hoveredObj.enabled = !clicked;
        hovered = true;
    }

    public override void cOnMouseExit()
    {
        hoveredObj.enabled = false;
        normalObj.enabled = !clicked;
        hovered = false;
    }
}
