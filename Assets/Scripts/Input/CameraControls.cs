using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraControls : Singleton<CameraControls>
{
    public Vector3 Focus;
    public float Distance;

    public float RotationalSensitivity = 1.0f;

    // Use this for initialization
    void Start()
    {
        if(Init(this))
        {
            this.transform.position = Distance * Vector3.one.normalized;
            this.transform.LookAt(Focus);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton((int)MouseButton.Right))
        {
            this.transform.RotateAround(Focus, Vector3.up, Input.GetAxis("Horizontal"));
            this.transform.RotateAround(Focus, transform.right, -Input.GetAxis("Vertical"));
        }
    }
}
