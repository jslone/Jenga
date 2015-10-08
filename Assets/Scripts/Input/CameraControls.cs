using UnityEngine;
using System.Collections;

public class CameraControls : Singleton<CameraControls>
{
    public FollowBlocks followControls;
    public Vector3 Focus { get { return transform.parent.position; } }
    public float Distance;
    public Camera Camera;

    public float RotationalSensitivity = 1.0f;
    public float HandRotationalSensitivity = 200.0f;
    public float ZoomSensitivity = 1.0f;
    public float HandZoomSensitivity = 30.0f;
    public float ZoomNearClip = 0.1f;
    public float ZoomFarClip = 10.0f;
    public float MinHeight = 0.1f;
    private Vector3 _positionCache;
    private Quaternion _rotationCache;

    private Vector3 lastHandPosition;
    public bool usingHand { get; private set; }

    // Use this for initialization
    void Awake()
    {

        if (Init(this))
        {
            Distance = this.transform.localPosition.magnitude;
            this.transform.LookAt(Focus);
        }
        usingHand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton((int)MouseButton.Right))
        {
            followControls.CurrentHeldBias = 1.0f;
            RotateHorizontal(Input.GetAxis("Horizontal"));
            RotateVertical(-Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Zoom") != 0)
        {
            Zoom(ZoomSensitivity * Input.GetAxis("Zoom"));
        }

        if (HandControls.Instance.isAltDown || HandControls.Instance.isAltHeld)
        {
            if (!HandControls.Instance.isHoldindSomething())
            {
                if (!usingHand)
                {
                    lastHandPosition = HandControls.Instance.LocalPosition;
                    usingHand = true;
                }
                else
                {
                    Vector3 delta = HandControls.Instance.LocalPosition - lastHandPosition;
                    delta.z = (HandZoomSensitivity * delta.z) * Mathf.Abs(delta.z);

                    RotateHorizontal(HandRotationalSensitivity * delta.x);
                    RotateVertical(HandRotationalSensitivity * -delta.y);

                    Zoom(HandZoomSensitivity * delta.z);
                    lastHandPosition = HandControls.Instance.LocalPosition;
                }
            }
            else
            {
                usingHand = false;
            }

        }
        else
        {
            usingHand = false;
        }
    }

    private void RotateHorizontal(float degree)
    {
        this.transform.RotateAround(Focus, Vector3.up, degree);
    }

    private void RotateVertical(float degree)
    {
        Vector3 pos = transform.localPosition;
        Quaternion rot = transform.localRotation;

        this.transform.RotateAround(Focus, transform.right, degree);
        if (transform.position.y <= 0 || Vector3.Dot(transform.up, Vector3.up) < 0)
        {
            transform.localPosition = pos;
            transform.localRotation = rot;
        }
    }

    private void Zoom(float delta)
    {
        float newDistance = Mathf.Clamp(Distance + delta, ZoomNearClip, ZoomFarClip);
        transform.position = (this.transform.position - Focus).normalized * newDistance + Focus;

        if (transform.position.y < MinHeight)
        {
            this.transform.position = (this.transform.position - Focus).normalized * Distance + Focus;
        }
        else
        {
            Distance = newDistance;
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
