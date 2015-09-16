using UnityEngine;
using System.Collections;

public class FixArrowScale : MonoBehaviour
{
    private Vector3 scale;

    // Use this for initialization
    void Start()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localScale = transform.localScale;
        // confusing rotation here
        localScale.z = scale.z / transform.parent.localScale.y;
        transform.localScale = localScale;
    }

}
