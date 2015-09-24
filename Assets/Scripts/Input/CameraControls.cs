using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraControls : Singleton<CameraControls>
{
    public FollowBlocks followControls;
    public Vector3 Focus { get { return transform.parent.position; } }
    public float Distance;
    public Camera Camera;

    public float RotationalSensitivity = 1.0f;
    public float ZoomSensitivity = 1.0f;
    public float ZoomNearClip = 0.1f;

    private Vector3 _positionCache;
    private Quaternion _rotationCache;

    // Use this for initialization
    void Start()
    {
        
        if(Init(this))
        {
            this.Camera = GetComponent<Camera>();
            this.transform.position = Distance * Vector3.one.normalized + Focus;
            this.transform.LookAt(Focus);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton((int)MouseButton.Right))
        {
            followControls.CurrentHeldBias = 1.0f;
            this.transform.RotateAround(Focus, Vector3.up, Input.GetAxis("Horizontal"));
            this.transform.RotateAround(Focus, transform.right, -Input.GetAxis("Vertical"));
        }
        if(Input.GetAxis("Zoom") != 0)
        {
            Distance += ZoomSensitivity * Input.GetAxis("Zoom");
            Distance = Mathf.Max(ZoomNearClip, Distance);
            this.transform.position = (this.transform.position - Focus).normalized * Distance + Focus;
        }
    }

    public Vector3 GetClosestDirection()
    {
        Vector3[] directions =
        {
            Vector3.up,
            Vector3.down,
            Vector3.right,
            Vector3.left,
            Vector3.forward,
            Vector3.back,
        };


        Vector3 fromFocus = this.transform.position - Focus;
        Vector3 closest = directions[0];
        foreach (Vector3 dir in directions)
        {
            if (Vector3.Dot(fromFocus, dir) > Vector3.Dot(fromFocus, closest))
            {
                closest = dir;
            }
        }

        return closest;
    }


    public void Snap()
    {
        _positionCache = this.transform.position;
        _rotationCache = this.transform.rotation;

        transform.position = Distance * GetClosestDirection();
        transform.LookAt(Focus);
    }

    public void Unsnap()
    {
        transform.position = _positionCache;
        transform.rotation = _rotationCache;
    }
}
