using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class DragableBlock : MouseInteractable
{
    public static List<DragableBlock> held;
    public PhysicMaterial heldBlockMaterial;
    public PhysicMaterial unheldBlockMaterial;
    public float heldDrag = 10.0f;
    private float unheldDrag;
    public float followAcceleration = 2.0f;

    new Rigidbody rigidbody;
    public Pointer heldBy { get; private set; }
    private Vector3 offset = Vector3.zero;
    void Awake()
    {
        if(held == null)
        {
            held = new List<DragableBlock>();
        }
    }

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        unheldDrag = rigidbody.drag;
    }

    void FixedUpdate()
    {
        if (heldBy != null)
        {
            Vector3 force = (heldBy.WorldPosition - (rigidbody.position + offset)) * Time.fixedDeltaTime * heldDrag;
            rigidbody.AddForce(force, ForceMode.Impulse);
            FollowBlocks.Instance.CurrentHeldBias += followAcceleration * force.magnitude;
        }
    }

    void SetPieceHeld(bool held, bool isMouse = false)
    {
        rigidbody.useGravity = !held;
        rigidbody.freezeRotation = held;
        rigidbody.GetComponent<Collider>().material = held ? heldBlockMaterial : unheldBlockMaterial;
        rigidbody.transform.FindChild("whenHeld").gameObject.SetActive(held && isMouse);
        rigidbody.drag = held ? heldDrag : unheldDrag;
        if(held)
        {
            DragableBlock.held.Add(this);
        }
        else
        {
            DragableBlock.held.Remove(this);
        }
    }

    public override void cOnMouseDown(Pointer p)
    {
        heldBy = p;
        offset = p.WorldPosition - transform.position;
        FollowBlocks.Instance.CurrentHeldBias = 1;
        SetPieceHeld(true, p == Mouse.Instance);
    }

    public override void cOnMouseUp()
    {
        heldBy = null;
        SetPieceHeld(false);
    }

    void OnDestroy()
    {
        held.Remove(this);
    }
}
