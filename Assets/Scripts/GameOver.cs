using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    public PlanarControls grabber;
    HashSet<Rigidbody> onTable = new HashSet<Rigidbody>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (onTable.Count > 4 || (onTable.Count == 4 && !onTable.Contains(grabber.heldPiece)))
        {
            // now ya done son
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject block in blocks)
            {
                float force = Application.isEditor ? 100.0f : 25.0f;
                block.GetComponent<Rigidbody>().AddExplosionForce(force, CameraControls.Instance.Focus, force);
                block.GetComponent<Rigidbody>().AddExplosionForce(2 * force, Vector3.zero, force);
                this.enabled = false;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            onTable.Add(col.rigidbody);
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            onTable.Remove(col.rigidbody);
        }
    }
}
