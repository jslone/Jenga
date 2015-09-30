using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class HandControls : Singleton<HandControls>, Pointer
{
    public HandController handController;
    public float PinchTolerance = 0.9f;

    private Hand currentHand;

    private Ray zero = new Ray();
    private Ray handRay = new Ray();
    
    void Awake()
    {
        Init(this);
    }

    // Use this for initialization
    void Start()
    {
        zero.origin = Vector3.zero;
        zero.direction = Vector3.zero;
        PointerManager.Instance.pointers.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = handController.GetFrame();
        HandList hands = frame.Hands;

        if(isUp) isUp = false;
        if(isDown) isDown = false;

        if (hands.Count > 0)
        {
            currentHand = hands[0];

            handRay.origin = handController.transform.TransformPoint(currentHand.PalmPosition.ToUnityScaled());

            Vector3 centerOfFingers = Vector3.zero;
            foreach(Finger f in currentHand.Fingers)
            {
                centerOfFingers += f.TipPosition.ToUnityScaled();
            }
            centerOfFingers /= currentHand.Fingers.Count;

            centerOfFingers = handController.transform.TransformPoint(centerOfFingers);

            handRay.direction = (centerOfFingers - handRay.origin).normalized;

            Debug.DrawRay(handRay.origin, 10 * handRay.direction, Color.red);

            if(currentHand.PinchStrength >= PinchTolerance && !isHeld)
            {
                isDown = true;
                isHeld = true;
            }
            else if (currentHand.PinchStrength < PinchTolerance && isHeld)
            {
                isUp = true;
                isHeld = false;
            }
        }
        else
        {
            currentHand = null;
        }
    }

    public MouseInteractable LastOver { get; set; }
    public Ray Ray { get { return currentHand == null ? zero : handRay; } }
    public bool isDown { get; private set; }
    public bool isUp { get; private set; }
    public bool isHeld { get; private set; }
}
