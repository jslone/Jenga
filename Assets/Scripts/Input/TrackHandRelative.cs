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
            transform.localPosition = Vector3.Lerp(transform.localPosition, scale * (HandControls.Instance.LocalPosition + offset), Time.deltaTime * Speed);
        } else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * Speed);
        }
    }

}
