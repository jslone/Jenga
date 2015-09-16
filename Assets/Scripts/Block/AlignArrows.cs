using UnityEngine;
using System.Collections;

public class AlignArrows : MonoBehaviour
{
    public MaximalAngleToggle worldPlanes;
    public MaximalAngleToggle localPlanes;

    // Use this for initialization
    void Start()
    {
        worldPlanes = GameObject.Find("WorldSnap").GetComponent<MaximalAngleToggle>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(PlanarControls.Instance.CurrentSpace)
        {
            case PlanarControls.Spaces.World:
                transform.rotation = worldPlanes.CurrentAxis.transform.rotation;
                break;
            case PlanarControls.Spaces.Local:
                transform.rotation = localPlanes.CurrentAxis.transform.rotation;
                break;
            case PlanarControls.Spaces.Camera:
                transform.rotation = CameraControls.Instance.transform.rotation;
                break;
        }
    }

}
