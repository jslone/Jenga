using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FollowHand : MouseInteractable
{
    public PhysicMaterial heldBlockMaterial;
    public PhysicMaterial unheldBlockMaterial;

    new Rigidbody rigidbody;
    private bool heldByHand = false;
    private Vector3 delta = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (heldByHand) delta = HandControls.Instance.Delta;
    }

    void FixedUpdate()
    {
        if(heldByHand)
        {
            rigidbody.velocity = delta / Time.fixedDeltaTime;
            delta = Vector3.zero;
        }
    }

    void SetPieceHeld(bool held)
    {
        heldByHand = held;
        rigidbody.useGravity = !held;
        rigidbody.freezeRotation = held;
        rigidbody.GetComponent<Collider>().material = held ? heldBlockMaterial : unheldBlockMaterial;
        rigidbody.transform.FindChild("whenHeld").gameObject.SetActive(held);
    }

    public override void cOnMouseDown()
    {
        SetPieceHeld(true);
    }

    public override void cOnMouseUp()
    {
        SetPieceHeld(false);
    }
}
