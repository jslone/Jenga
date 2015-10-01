using UnityEngine;
using System.Collections;

public class FollowBlocks : Singleton<FollowBlocks>
{
    public float CurrentHeldBias = 1.0f;
    public float MaxHeldBias = 10.0f;
    public float tunedDistance = 4.5f;
    public float handBias = 10.0f;
    GameObject[] blocks;
    public Transform handTracker;

    void Awake()
    {
        Init(this);
    }

    // Use this for initialization
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 com = Vector3.zero;
        foreach (GameObject block in blocks)
        {
            com += block.transform.position;
        }

        float bias = (Mathf.Min(CurrentHeldBias, MaxHeldBias) - 1.0f) * (tunedDistance / CameraControls.Instance.Distance);

        foreach(DragableBlock block in DragableBlock.held)
        {
            com += bias * block.transform.position;
        }

        if(HandControls.Instance.Active)
        {
            com += handBias * handTracker.localPosition;
        }

        com /= blocks.Length;
        transform.position = Vector3.Lerp(transform.position, com, Time.deltaTime);
        
    }
}
