using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class DragableBlock : MouseInteractable
{
    public static List<DragableBlock> held;
    public PhysicMaterial heldBlockMaterial;
    public PhysicMaterial unheldBlockMaterial;
    public float heldDragMultiplier = 10.0f;
    public float followAcceleration = 2.0f;

    new Rigidbody rigidbody;
    private Pointer heldBy = null;
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
    }

    void FixedUpdate()
    {
        if (heldBy != null)
        {
            Vector3 force = (heldBy.WorldPosition - (rigidbody.position + offset)) * Time.fixedDeltaTime * heldDragMultiplier;
            rigidbody.AddForce(force, ForceMode.Impulse);
            FollowBlocks.Instance.CurrentHeldBias += followAcceleration * force.magnitude;
        }
    }

    void SetPieceHeld(bool held)
    {
        rigidbody.useGravity = !held;
        rigidbody.freezeRotation = held;
        rigidbody.GetComponent<Collider>().material = held ? heldBlockMaterial : unheldBlockMaterial;
        rigidbody.transform.FindChild("whenHeld").gameObject.SetActive(held);
        
        if(held)
        {
            rigidbody.drag *= heldDragMultiplier;
            DragableBlock.held.Add(this);
        }
        else
        {
            rigidbody.drag /= heldDragMultiplier;
            DragableBlock.held.Remove(this);
        }
    }

    public override void cOnMouseDown(Pointer p)
    {
        heldBy = p;
        offset = p.WorldPosition - transform.position;
        FollowBlocks.Instance.CurrentHeldBias = 1;
        SetPieceHeld(true);
    }

    public override void cOnMouseUp()
    {
        heldBy = null;
        SetPieceHeld(false);
    }
}
