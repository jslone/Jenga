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
                block.GetComponent<Rigidbody>().AddExplosionForce(100, CameraControls.Instance.Focus, 100);
                block.GetComponent<Rigidbody>().AddExplosionForce(200, Vector3.zero, 100);
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
