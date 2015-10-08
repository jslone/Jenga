using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MouseInteractable : MonoBehaviour
{
    public virtual void cOnMouseEnter()
    { }

    public virtual void cOnMouseEnter(Pointer p)
    { }

    public void iOnMouseEnter(Pointer p)
    {
        cOnMouseEnter();
        cOnMouseEnter(p);
    }

    public virtual void cOnMouseExit()
    { }

    public virtual void cOnMouseExit(Pointer p)
    { }

    public void iOnMouseExit(Pointer p)
    {
        cOnMouseExit();
        cOnMouseExit(p);
    }

    public virtual void cOnMouseStay()
    { }

    public virtual void cOnMouseStay(Pointer p)
    { }

    public void iOnMouseStay(Pointer p)
    {
        cOnMouseStay();
        cOnMouseStay(p);
    }

    public virtual void cOnMouseDown()
    { }

    public virtual void cOnMouseDown(Pointer p)
    { }

    public void iOnMouseDown(Pointer p)
    {
        cOnMouseDown();
        cOnMouseDown(p);
    }

    public virtual void cOnMouseUp()
    { }

    public virtual void cOnMouseUp(Pointer p)
    { }

    public void iOnMouseUp(Pointer p)
    {
        cOnMouseUp();
        cOnMouseUp(p);
    }

    public virtual void cOnMouseHold()
    { }

    public virtual void cOnMouseHold(Pointer p)
    { }

    public void iOnMouseHold(Pointer p)
    {
        cOnMouseHold();
        cOnMouseHold(p);
    }
}

public interface Pointer
{
    Collider LastOver { get; set; }
    Collider CurrentOver { get; }
    MouseInteractable[] LastClicked { get; set; }
    Ray Ray { get; }
    bool isDown { get; }
    bool isUp { get; }
    bool isHeld { get; }
    bool Active { get; }
    Vector3 WorldPosition { get; }
}

static class PointerExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumeration, System.Action<T> action)
    {
        if (enumeration == null) return;
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static bool isHoldindBlock(this Pointer p)
    {
        return p.LastClicked != null && p.LastClicked.Where(i => i is DragableBlock).Count() > 0;
    }

    public static bool isHoldindSomething(this Pointer p)
    {
        return p.LastClicked != null && p.LastClicked.Length > 0;
    }
}

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
            if (p.Active) UpdateInteractibles(p);
            else Release(p);
        }
    }

    void UpdateInteractibles(Pointer pointer)
    {
        Collider lastOver = pointer.LastOver;

        Collider hit = pointer.CurrentOver;
        if (hit != null)
        {
            MouseInteractable[] interactibles = hit.GetComponentsInChildren<MouseInteractable>();
            
            if (hit != lastOver)
            {
                if (lastOver != null)
                {
                    lastOver.GetComponentsInChildren<MouseInteractable>().ForEach(i => i.iOnMouseExit(pointer));
                }
                interactibles.ForEach(i => i.iOnMouseEnter(pointer));
                pointer.LastOver = hit;
            }
            else
            {
                interactibles.ForEach(i => i.iOnMouseStay(pointer));
            }
            if (pointer.isDown)
            {
                interactibles.ForEach(i => i.iOnMouseDown(pointer));
                pointer.LastClicked = interactibles;
            }
            if (pointer.isHeld)
            {
                interactibles.ForEach(i => i.iOnMouseHold(pointer));
            }
        }
        else
        {
            if (pointer.LastOver != null)
            {
                pointer.LastOver.GetComponentsInChildren<MouseInteractable>().ForEach(i => i.iOnMouseExit(pointer));
                pointer.LastOver = null;
            }
        }

        if (pointer.isUp)
        {
            pointer.LastClicked.ForEach(i => i.iOnMouseUp(pointer));
            pointer.LastClicked = null;
        }
    }

    void Release(Pointer pointer)
    {
        if(pointer.LastOver != null)
        {
            pointer.LastOver.GetComponentsInChildren<MouseInteractable>().ForEach(i => i.iOnMouseExit(pointer));
            pointer.LastOver = null;
        }

        pointer.LastClicked.ForEach(i => i.iOnMouseUp(pointer));
        pointer.LastClicked = null;
    }
}
