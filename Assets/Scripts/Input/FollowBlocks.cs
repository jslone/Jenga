using UnityEngine;
using System.Collections;

public class FollowBlocks : Singleton<FollowBlocks>
{
    public PlanarControls grabber;
    public float CurrentHeldBias = 1.0f;
    public float MaxHeldBias = 3.0f;
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

        foreach(DragableBlock block in DragableBlock.held)
        {
            com += (Mathf.Min(CurrentHeldBias, MaxHeldBias) - 1.0f) * block.transform.position;
        }

        com /= blocks.Length;
        transform.position = Vector3.Lerp(transform.position, com, Time.deltaTime);
        
    }
}
