using UnityEngine;
using System.Collections;

public class TrackHandRelative : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Scale = 1.0f;
    public float CalibratedDistance = 4.5f;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float scale = Scale * transform.parent.position.magnitude / CalibratedDistance;
        if(HandControls.Instance.Active)
        {
            if(CameraControls.Instance.usingHand)
            {
                //dragging camera
            }
            else
            {
                Vector3 delta = (HandControls.Instance.LocalPosition - HandControls.Instance.BasePosition);
                Vector3 localPosition = Vector3.Lerp(transform.localPosition, scale * (delta + offset), Time.deltaTime * Speed);
                Vector3 position = transform.parent.TransformPoint(localPosition);

                Vector2 around = new Vector2(position.x, position.z);
                float distance = around.sqrMagnitude;

                if (distance == 0)
                {
                    return;
                }
                if (distance < 1)
                {
                    around.Normalize();
                }

                position.x = around.x;
                position.z = around.y;
                position.y = Mathf.Max(CameraControls.Instance.MinHeight, position.y);
                transform.position = position;
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * Speed);
        }
    }

}
