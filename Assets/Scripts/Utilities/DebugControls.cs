using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour
{
    bool logFPSDrops = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            logFPSDrops = !logFPSDrops;
        }

        if(logFPSDrops && Time.deltaTime > 1.0f/75)
        {
            Debug.Log("FPS crash: " + 1.0f / Time.deltaTime);
        }
    }
}
