﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneVRMovementScript : MonoBehaviour {

    private Rigidbody drone;

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        //drone = GetComponent<Rigidbody>();
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        //MovementUpDown();

        //drone.AddRelativeForce(Vector3.up * upForce);
    }

    public float upForce;
    void MovementUpDown()
    {
       upForce = 98.1f;
    }

    private float movementForwardSpeed = 500.0f;
    [HideInInspector]
    public float tiltAmountForward = 0;
    private float tiltVelocityForward;
    void MovementForward()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            drone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
        }
    }

    private float movementRightSpeed = 300.0f;
    private float tiltAmountRight;
    private float tiltVelocityRight;
    void MovementRight()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            drone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * movementRightSpeed);
            tiltAmountRight = Mathf.SmoothDamp(tiltAmountRight, -20 * Input.GetAxis("Horizontal"), ref tiltVelocityRight, 0.1f);
        }
        else
        {
            tiltAmountRight = Mathf.SmoothDamp(tiltAmountRight, 0, ref tiltVelocityRight, 0.1f);
        }
    }

    [HideInInspector]
    public float currentYRotation;
    private float wantedYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationYVelocity;

    void Rotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }

    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f) // 0, s = -1, w = 1
        {
            drone.velocity = Vector3.ClampMagnitude(drone.velocity, Mathf.Lerp(drone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            drone.velocity = Vector3.ClampMagnitude(drone.velocity, Mathf.Lerp(drone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            drone.velocity = Vector3.ClampMagnitude(drone.velocity, Mathf.Lerp(drone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            drone.velocity = Vector3.SmoothDamp(drone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }
}
