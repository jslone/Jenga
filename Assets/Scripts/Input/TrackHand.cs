using UnityEngine;
using System.Collections;

public class TrackHand : MonoBehaviour
{
    new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
        float size = HandControls.Instance.SelectRadius;
        transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update()
    {
        renderer.enabled = HandControls.Instance.Active;
        if(HandControls.Instance.Active)
        {
            transform.position = HandControls.Instance.betweenFingers;
        }
    }
}
