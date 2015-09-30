using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    Collider LastOver { get; set; }
    Collider CurrentOver { get; }
    MouseInteractable[] LastClicked { get; set; }
    Ray Ray { get; }
    bool isDown { get; }
    bool isUp { get; }
    bool isHeld { get; }
    bool Active { get; }
}

static class ForEachExtension
{
    public static void ForEach<T>(this IEnumerable<T> enumeration, System.Action<T> action)
    {
        if (enumeration == null) return;
        foreach (T item in enumeration)
        {
            action(item);
        }
    }
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
            if(p.Active) UpdateInteractibles(p);
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
                    lastOver.GetComponentsInChildren<MouseInteractable>().ForEach(i => i.cOnMouseExit());
                }
                interactibles.ForEach(i => i.cOnMouseEnter());
                pointer.LastOver = hit;
            }
            else
            {
                interactibles.ForEach(i => i.cOnMouseStay());
            }
            if (pointer.isDown)
            {
                interactibles.ForEach(i => i.cOnMouseDown());
                pointer.LastClicked = interactibles;
            }
            if (pointer.isHeld)
            {
                interactibles.ForEach(i => i.cOnMouseHold());
            }
        }
        else
        {
            if (lastOver != null) lastOver.GetComponentsInChildren<MouseInteractable>().ForEach(i => i.cOnMouseExit());
            pointer.LastOver = null;
        }

        if (pointer.isUp)
        {
            pointer.LastClicked.ForEach(i => i.cOnMouseUp());
            pointer.LastClicked = null;
        }
    }
}
