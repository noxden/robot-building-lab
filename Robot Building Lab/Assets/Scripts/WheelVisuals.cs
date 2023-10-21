using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisuals : MonoBehaviour
{
    WheelCollider wheelCol;
    Vector3 wheelPos;
    Quaternion wheelRot;
    void Start()
    {
        wheelCol = GetComponentInParent<WheelCollider>();

    }

    void Update()
    {
        
        wheelCol.GetWorldPose(out wheelPos, out wheelRot);

        transform.position = wheelPos;
        transform.rotation = wheelRot;
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y,90);
    }
}
