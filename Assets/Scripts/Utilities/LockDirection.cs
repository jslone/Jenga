using UnityEngine;
using System.Collections;

public class LockDirection : MonoBehaviour
{
    public bool up;
    public bool right;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.up = up ? Vector3.up : transform.parent.up;
        transform.right = right ? Vector3.right : transform.parent.right;
    }
}
