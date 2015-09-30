using UnityEngine;
using System.Collections;

public class TrackHand : MonoBehaviour
{
    public float maxDistance = 5.0f;
    new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.enabled = HandControls.Instance.Active;
        if(HandControls.Instance.Active)
        {
            RaycastHit info;
            if (Physics.Raycast(HandControls.Instance.Ray, out info, maxDistance))
            {
                transform.position = info.point;
            }
            else
            {
                transform.position = HandControls.Instance.Ray.origin + maxDistance * HandControls.Instance.Ray.direction;
            }
        }
    }
}
