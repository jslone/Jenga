using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            float force = 7.5f;
            block.GetComponent<Rigidbody>().AddExplosionForce(force, CameraControls.Instance.Focus, force);
            block.GetComponent<Rigidbody>().AddExplosionForce(2 * force, Vector3.zero, force);
            this.enabled = false;
        }
    }
}
