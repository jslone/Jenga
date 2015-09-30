using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseInteractable : MonoBehaviour
{
    public virtual void cOnMouseEnter()
    { }

    public virtual void cOnMouseExit()
    { }

    public virtual void cOnMouseStay()
    { }

    public virtual void cOnMouseDown()
    { }

    public virtual void cOnMouseUp()
    { }

    public virtual void cOnMouseHold()
    { }
}

public interface Pointer
{
    MouseInteractable LastOver { get; set; }
    Ray Ray { get; }
    bool isDown { get; }
    bool isUp { get; }
    bool isHeld { get; }
}

[RequireComponent(typeof(RectTransform))]
public class PointerManager : Singleton<PointerManager>
{
    public List<Pointer> pointers = new List<Pointer>();

    void Awake()
    {
        Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Pointer p in pointers)
        {
            UpdateInteractibles(p);
        }
    }

    void UpdateInteractibles(Pointer pointer)
    {
        MouseInteractable lastOver = pointer.LastOver;

        RaycastHit hitInfo;
        if (Physics.Raycast(pointer.Ray, out hitInfo))
        {
            MouseInteractable hit = hitInfo.collider.GetComponent<MouseInteractable>();
            
            if (hit != lastOver)
            {
                if (lastOver != null)
                {
                    lastOver.cOnMouseExit();
                }
                if (hit != null)
                {
                    hit.cOnMouseEnter();
                }
                pointer.LastOver = hit;
            }
            if (lastOver != null)
            {
                lastOver.cOnMouseStay();
                if (pointer.isDown)
                {
                    lastOver.cOnMouseDown();
                }
                if (pointer.isUp)
                {
                    lastOver.cOnMouseUp();
                }
                if (pointer.isHeld)
                {
                    lastOver.cOnMouseHold();
                }
            }
        }
        else
        {
            if (lastOver != null) lastOver.cOnMouseExit();
            pointer.LastOver = null;
        }
    }
}
