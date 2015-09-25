using UnityEngine;
using System.Collections;

public class FixPlayerScale : MonoBehaviour
{
    public float PlayerScale = 1.0f;
    
    // Use this for initialization
    void Start()
    {
        if (!Application.isEditor)
        {
            transform.localScale *= PlayerScale;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
