using UnityEngine;
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

[RequireComponent(typeof(AudioSource))]
public class ScoreKeeper : MonoBehaviour {

    /*
        Score is based on highest block with no velocity.
        Does not count held piece.
    */

    public Text text;
    public int score;
    public GameObject confetti;

    private float maxY;
    private int lastScore;

    private GameObject[] tower;
    private GameObject highestBlock;
    private Stopwatch timer = new Stopwatch();
    private AudioSource yayAudio;

	// Use this for initialization
	void Start () {
        yayAudio = this.GetComponent<AudioSource>();
        tower = GameObject.FindGameObjectsWithTag("Block");
        maxY = 0.0f;
        lastScore = 0;

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
                Instantiate(confetti, cTrans.position, Quaternion.identity);
                if (score - lastScore > 400)
                {
                    yayAudio.Play();
                    lastScore = score;
                }
            } 
        }

        if (!timer.IsRunning) timer.Start();

    }

    // Update Score
    int ComputeScore(float sc) {
        // stop timer
        timer.Stop();

        int b = (int)sc * 100;

        int elapsed = (int) timer.getTimeSeconds();

        if (elapsed > 20) elapsed = 20;

        timer.Reset();
        return b + (20 - elapsed) * 5;
    }
}
