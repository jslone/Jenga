using UnityEngine;
using System.Collections;

public class FixPlayerBug : MonoBehaviour
{
    public static bool hasLoadedOnce = false;
    
    // Use this for initialization
    void Start()
    {
        if (!Application.isEditor && !hasLoadedOnce)
        {
            hasLoadedOnce = true;
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
