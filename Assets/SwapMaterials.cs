using UnityEngine;
using System.Collections;

public class SwapMaterials : MouseInteractable
{
    public Material[] clickedMaterials;
    public Material[] hoverMaterials;
    private Material[] materialsCache;

    new private Renderer renderer;

    private bool clicked = false;
    private bool hovered = false;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        materialsCache = renderer.materials;
    }

    public override void cOnMouseDown()
    {
        renderer.materials = clickedMaterials;
        clicked = true;
    }

    public override void cOnMouseUp()
    {
        renderer.materials = hovered ? hoverMaterials : materialsCache;
        clicked = false;
    }

    public override void cOnMouseEnter()
    {
        if (!clicked)
        {
            renderer.materials = hoverMaterials;
        }
        hovered = true;
    }

    public override void cOnMouseExit()
    {
        if(!clicked)
            renderer.materials = materialsCache;
        hovered = false;
    }
}
