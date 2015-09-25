using UnityEngine;
using System.Collections;

public class FastForward : MonoBehaviour
{
    public float Duration;
    public float Scale;

    private float startTime;
    private float originalTimeScale;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        originalTimeScale = Time.timeScale;
        Time.timeScale = Scale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > Duration)
        {
            Time.timeScale = originalTimeScale;
            this.enabled = false;
        }
    }
}
