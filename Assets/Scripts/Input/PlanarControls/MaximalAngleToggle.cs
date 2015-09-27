using UnityEngine;
using System.Collections;

public class MaximalAngleToggle : MonoBehaviour
{
    public Collider[] SnapAxes;

    int currentAxisIndex = 0;
    public Collider CurrentAxis { get { return SnapAxes[currentAxisIndex]; } }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float maximalCos0 = Mathf.Abs(Vector3.Dot(CurrentAxis.transform.forward, CameraControls.Instance.transform.forward));
        for(int i = 0; i < SnapAxes.Length; i++)
        {
            SnapAxes[i].gameObject.SetActive(false);

            float cos0 = Vector3.Dot(SnapAxes[i].transform.forward, CameraControls.Instance.transform.forward);
            float absCos0 = Mathf.Abs(cos0);
            if(absCos0 > maximalCos0)
            {
                maximalCos0 = absCos0;
                currentAxisIndex = i;
            }
        }

        SnapAxes[currentAxisIndex].gameObject.SetActive(true);
    }

}
