using UnityEngine;
using System.Collections;

public class FollowBlocks : Singleton<FollowBlocks>
{
    public float CurrentHeldBias = 1.0f;
    public float MaxHeldBias = 10.0f;
    public float tunedDistance = 4.5f;
    public float handBias = 10.0f;
    public float minHeight = 1.2f;
    public GameObject Reset;
    public GameObject Exit;
    GameObject[] blocks;
    public Transform handTracker;
    GameOver go;

    void Awake()
    {
        Init(this);
    }

    // Use this for initialization
    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        go = GameObject.FindObjectOfType<GameOver>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 com = Vector3.zero;
        if (go.enabled)
        {
            foreach (GameObject block in blocks)
            {
                com += block.transform.position;
            }

            float bias = (Mathf.Min(CurrentHeldBias, MaxHeldBias) - 1.0f) * (tunedDistance / CameraControls.Instance.Distance);

            foreach (DragableBlock block in DragableBlock.held)
            {
                com += bias * block.transform.position;
            }

            if (HandControls.Instance.Active)
            {
                com += handBias * handTracker.localPosition;
            }

            com /= blocks.Length;
        }
        else
        {
            com = 0.5f * Reset.transform.position + 0.5f * Exit.transform.position;
        }

        com.y = Mathf.Max(minHeight, com.y);
        transform.position = Vector3.Lerp(transform.position, com, (go.enabled ? 1.0f : 0.5f) * Time.deltaTime);
        
    }
}
