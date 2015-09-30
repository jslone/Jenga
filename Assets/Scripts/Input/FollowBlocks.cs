using UnityEngine;
using System.Collections;

public class FollowBlocks : Singleton<FollowBlocks>
{
    public float CurrentHeldBias = 1.0f;
    public float MaxHeldBias = 10.0f;
    public float tunedDistance = 4.5f;
    GameObject[] blocks;

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

        com /= blocks.Length;
        transform.position = Vector3.Lerp(transform.position, com, Time.deltaTime);
        
    }
}
