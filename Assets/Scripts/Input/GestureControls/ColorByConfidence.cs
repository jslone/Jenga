using UnityEngine;
using System.Collections;
using Leap;

public class ColorByConfidence : MonoBehaviour
{
    public RiggedHand hand;
    public Color BadColor;
    private Color GoodColor;
    private Renderer rend;
    
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        GoodColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.color = Color.Lerp(BadColor, GoodColor, hand.GetLeapHand().Confidence);
    }
}
