﻿using UnityEngine;
using System.Collections;

public class FollowBlocks : MonoBehaviour
{
    public PlanarControls grabber;
    public float CurrentHeldBias = 1.0f;
    public float MaxHeldBias = 3.0f;
    GameObject[] blocks;

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

        if (grabber.heldPiece != null)
        {
            com += (Mathf.Min(CurrentHeldBias,MaxHeldBias) - 1.0f) * grabber.heldPiece.position;
        }

        com /= blocks.Length;
        transform.position = Vector3.Lerp(transform.position, com, Time.deltaTime);
    }
}