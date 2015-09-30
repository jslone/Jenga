﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Delta = Vector3.zero;

        if (hands.Count > 0)
        {
            if(currentHand != hands[0])
            {
                currentHand = hands[0];
            }
            Finger indexFinger = currentHand.Fingers.Where(f => f.Type == Finger.FingerType.TYPE_INDEX).SingleOrDefault();
            if(indexFinger != null && indexFinger.IsValid)
            {
                Vector3 newPosition = indexFinger.StabilizedTipPosition.ToUnityScaled();
                if(Time.frameCount - 1 == lastFramePositionUpdated)
                {
                    Delta = handController.transform.TransformVector(newPosition - lastPosition);
                }
                else
                {
                    Delta = Vector3.zero;
                }
                Debug.Log(Delta);
                lastPosition = newPosition;
                lastFramePositionUpdated = Time.frameCount;
            }
            

            handRay.origin = handController.transform.TransformPoint(currentHand.PalmPosition.ToUnityScaled());

            Vector3 localDir = (currentHand.Direction.ToUnityScaled() + currentHand.PalmNormal.ToUnityScaled()).normalized;
            handRay.direction = handController.transform.TransformDirection(localDir);
            
            //int numberOfExtendedFingers = currentHand.Fingers.Where(f => f.IsExtended).Count();
            
            //Vector3 localDir = ((numberOfExtendedFingers + 1) * currentHand.Direction.ToUnityScaled() + 2 * currentHand.PalmNormal.ToUnityScaled()).normalized;
            //handRay.direction = handController.transform.TransformDirection(localDir);
            
            // COM based on finger tips, pinch becomes hard
            //Vector3 centerOfFingers = Vector3.zero;
            //foreach (Finger f in currentHand.Fingers)
            //{
            //    centerOfFingers += f.StabilizedTipPosition.ToUnityScaled();
            //}
            //centerOfFingers /= currentHand.Fingers.Count;

            //centerOfFingers = handController.transform.TransformPoint(centerOfFingers);

            //handRay.direction = (centerOfFingers - handRay.origin).normalized;

            // Promising, basically ends up using the thumb though, kind of shaky when pinching
            //Dictionary<Finger.FingerType, float> bias = new Dictionary<Finger.FingerType, float>()
            //{
            //    {Finger.FingerType.TYPE_THUMB, 6},
            //    {Finger.FingerType.TYPE_INDEX, 0},
            //    {Finger.FingerType.TYPE_MIDDLE, 1},
            //    {Finger.FingerType.TYPE_RING, 1},
            //    {Finger.FingerType.TYPE_PINKY, 1},
            //};

            //Vector3 dirOfFingers = Vector3.zero;
            //foreach (Finger f in currentHand.Fingers)
            //{
            //    dirOfFingers += bias[f.Type] * f.Direction.ToUnityScaled();
            //}
            //dirOfFingers.Normalize();

            //Vector3 localDir = (currentHand.Direction.ToUnityScaled() + dirOfFingers).normalized;
            //handRay.direction = handController.transform.TransformDirection(localDir);

            //Vector3 fingerBoneDir = Vector3.zero;
            //foreach (Finger f in currentHand.Fingers)
            //{
            //    fingerBoneDir += f.Bone(Bone.BoneType.TYPE_METACARPAL).Direction.ToUnityScaled();
            //}
            //fingerBoneDir.Normalize();

            //handRay.direction = handController.transform.TransformDirection(-fingerBoneDir);

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
            if (isHeld)
            {
                isHeld = false;
                isUp = true;
            }
        }
    }

    public Collider LastOver { get; set; }
    public Ray Ray { get { return currentHand == null ? zero : handRay; } }
    public bool isDown { get; private set; }
    public bool isUp { get; private set; }
    public bool isHeld { get; private set; }
    public bool Active { get { return currentHand != null; } }
    public MouseInteractable[] LastClicked {get; set; }
    public Vector3 Delta { get; private set; }

    private Vector3 lastPosition;
    private int lastFramePositionUpdated = -2;
}