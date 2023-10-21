using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: Sometimes unpredicted behaviour when attaching / detaching new building blocks
[RequireComponent(typeof(Rigidbody))]
public class BuildingBlock : MonoBehaviour
{
    private new Rigidbody rigidbody;
    [SerializeField] public bool isCoreBlock = false;
    private List<Connector> activeConnections = new();  //< Connections to core

    [SerializeField] public bool isSteeringWheel = false;
    [SerializeField] public bool isPoweredWheel = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public bool GetIsAttachedToCore()
    {
        return transform.root.GetComponent<BuildingBlock>().isCoreBlock;
    }

    /// <returns>
    /// Success of the operation. False if connector was already registered as active connection.
    /// </returns>
    public bool AddCoreConnection(Connector ownedConnector)
    {
        if (!activeConnections.Contains(ownedConnector))
        {
            activeConnections.Add(ownedConnector);

            if (isSteeringWheel)
            {
                transform.root.GetComponent<RobotController>().SteeringWheels.Add(gameObject.GetComponentInChildren<WheelCollider>());
            }
            else if (isPoweredWheel)
            {
                transform.root.GetComponent<RobotController>().AccelerationWheels.Add(gameObject.GetComponentInChildren<WheelCollider>());
            }

            Attach(ownedConnector.attachedConnector);

            return true;
        }

        Debug.LogWarning($"{this.name}:{ownedConnector.name} is already registered as an active core connection.");
        return false;
    }

    /// <returns>
    /// Success of the operation. False if connector was not registered as active connection previously.
    /// </returns>
    public bool RemoveCoreConnection(Connector connector)
    {
        bool success = activeConnections.Remove(connector);
        if (activeConnections.Count == 0)
        {
            if (isSteeringWheel)
            {
                transform.root.GetComponent<RobotController>().SteeringWheels.Remove(gameObject.GetComponentInChildren<WheelCollider>());
            }
            else if (isPoweredWheel)
            {
                transform.root.GetComponent<RobotController>().AccelerationWheels.Remove(gameObject.GetComponentInChildren<WheelCollider>());
            }

            Detach();
        }

        return success;
    }

    private void Attach(Connector targetConnector)
    {
        transform.SetParent(targetConnector.gameObject.transform, true);
        // rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
    }

    private void Detach()
    {
        transform.SetParent(null, true);
        // rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (GetIsAttachedToCore())
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.02f * transform.localScale.magnitude);
        }
    }
}
