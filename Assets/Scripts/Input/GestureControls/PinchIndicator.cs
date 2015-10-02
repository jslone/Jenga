using UnityEngine;
using System.Collections;

public class PinchIndicator : MonoBehaviour
{
    public Color PinchColor;
    private Color normalColor;

    private Renderer rend;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        normalColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(HandControls.Instance.isDown)
        {
            rend.material.color = PinchColor;
        }
        if(HandControls.Instance.isUp)
        {
            rend.material.color = normalColor;
        }
    }
}
