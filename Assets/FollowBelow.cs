using UnityEngine;
using System.Collections;

public class FollowBelow : MonoBehaviour
{
    public Transform toFollow;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = toFollow.TransformPoint(offset);

        transform.position = pos;
        transform.forward = toFollow.forward;
    }
}
