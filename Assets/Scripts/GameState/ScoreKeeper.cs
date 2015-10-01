﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

class Stopwatch
{

    public bool IsRunning = false;
    private float startTime;
    private float endTime;
    private float currentTime;

    public void Start()
    {
        IsRunning = true;
        startTime = Time.time;
        currentTime = startTime;
        endTime = startTime;
    }

    public void Stop()
    {
        endTime = Time.time;
        IsRunning = false;
    }

    public void Reset()
    {
        startTime = 0.0f;
        endTime = 0.0f;
        IsRunning = false;
    }

    public void Update() {
        currentTime = Time.time;
    }

    public float getElapsedSeconds()
    {
        return currentTime - startTime;
    }

    public float getTimeSeconds()
    {
        return endTime - startTime;
    }

}

public class ScoreKeeper : MonoBehaviour {

    /*
        Score is based on highest block with no velocity.
        Does not count held piece.
    */

    public Text text;
    private float maxY;
    public int score;

    private GameObject[] tower;
    private GameObject highestBlock;
    private Stopwatch timer = new Stopwatch();

	// Use this for initialization
	void Start () {
        tower = GameObject.FindGameObjectsWithTag("Block");
        maxY = 0.0f;

        // Find highest block
        foreach (GameObject block in tower) {
            Transform cTrans = block.GetComponent<Transform>();
            if (cTrans.position.y > maxY) {
                maxY = cTrans.position.y;
            }
        }


        timer.Start();
    }

    // Update is called once per frame
    void Update() {
        Transform cTrans;
        Rigidbody rb;
       
        
        foreach (GameObject block in tower) {
            if (highestBlock != null && block == highestBlock) continue;
            if (DragableBlock.held.Contains(block.GetComponent<DragableBlock>())) continue;

            cTrans = block.GetComponent<Transform>();
            rb = block.GetComponent<Rigidbody>();

            if (rb.velocity.y == 0.0f && cTrans.position.y > maxY) {
                highestBlock = block;
                maxY = cTrans.position.y;
                score += ComputeScore(maxY);
                text.text = score.ToString();     
            } 
        }

        if (!timer.IsRunning) timer.Start();

    }

    // Update Score
    int ComputeScore(float sc) {
        // stop timer
        timer.Stop();

        int b = (int)sc;
        int t = (int)((sc - b) * 10);

        int elapsed = (int) timer.getTimeSeconds();

        if (elapsed > 20) elapsed = 0;

        timer.Reset();
        return b * 100 + t + (20 - elapsed) * 5;
    }
}
