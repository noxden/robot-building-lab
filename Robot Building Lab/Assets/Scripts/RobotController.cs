using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private List<WheelCollider> SteeringWheels;
    [SerializeField] private List<WheelCollider> AccelerationWheels;

    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float breakingForce = 300f;
    [SerializeField] private float maxTurnAngle = 15f;
    [SerializeField] private float maxRotationSpeed = 4000f;
    [SerializeField] private float driveBackSpeed = -1000f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    private void FixedUpdate()
    {
        // acceleration old input
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        // brake old input
        if (Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = breakingForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }

        // steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

        UpdateSteeringWheels();
        UpdateAccelerationWheels();
    }

    private void UpdateSteeringWheels()
    {
        foreach (WheelCollider wheel in SteeringWheels)
        {
            // apply brakeforce
            wheel.brakeTorque = currentBrakeForce;
            //apply steeringangle
            wheel.steerAngle = currentTurnAngle;
            // clamp rotationspeed
            wheel.rotationSpeed = Mathf.Clamp(wheel.rotationSpeed, driveBackSpeed, maxRotationSpeed);
            // update visual
            UpdateWheel(wheel, wheel.transform);

        }
    }
    private void UpdateAccelerationWheels()
    {
        foreach (WheelCollider wheel in AccelerationWheels)
        {
            // apply brakeforce
            wheel.brakeTorque = currentBrakeForce;
            // Set accelerationForce
            wheel.motorTorque = currentAcceleration;
            // update visual
            UpdateWheel(wheel, wheel.transform);


        }
    }



    void UpdateWheel(WheelCollider col, Transform trans)
    {
        // Get wheel collider state.
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        // Set wheel transform state.
       // trans.position = position;
       // trans.rotation = rotation;
    }
}
