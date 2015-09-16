using UnityEngine;
using System.Collections;

public class ScaleAroundBlock : MonoBehaviour
{
    public Transform dimReference;
    public float distance = 1.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pointInDimRefSpace = distance * dimReference.InverseTransformDirection(transform.up);
        Vector3 pointInWorldSpace = dimReference.TransformPoint(pointInDimRefSpace);
        Vector3 pointInParentSpace = transform.parent.InverseTransformPoint(pointInWorldSpace);

        Vector3 scale = transform.localScale;
        scale.y = pointInParentSpace.magnitude;
        transform.localScale = scale;
    }

}
