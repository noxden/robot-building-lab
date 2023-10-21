using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: Multi-Path Connections to the core are still not possible
public class Connector : MonoBehaviour
{
    private static Vector3 colliderSize = new Vector3(0.02f, 0.02f, 0.02f);
    private new BoxCollider collider = null;
    [SerializeField] public BuildingBlock owner;
    [SerializeField] public Connector attachedConnector = null;

    // Start is called before the first frame update
    void Awake()
    {
        owner = GetComponentInParent<BuildingBlock>();
        CreateCollider();
    }

    private void CreateCollider()
    {
        collider = GetComponent<BoxCollider>();
        if (!collider)
            collider = gameObject.AddComponent<BoxCollider>();

        collider.size = colliderSize;
        collider.isTrigger = true;
    }

    private void ConnectTo(Connector otherConnector)
    {
        if (attachedConnector != null)
            return;

        attachedConnector = otherConnector;
        Debug.Log($"Connected [{owner.name}:{this.name}] to [{otherConnector.owner.name}:{otherConnector.name}]");

        if (attachedConnector.owner.GetIsAttachedToCore())
        {
            owner.AddCoreConnection(this);
            // owner.transform.SetParent(attachedConnector.gameObject.transform, true);
            // owner.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void DisconnectFromAttached()
    {
        if (attachedConnector == null)
            return;

        Connector connectorToDisconnectFrom = attachedConnector;
        attachedConnector = null;

        owner.RemoveCoreConnection(this);
        // owner.transform.SetParent(null, true);
        // owner.GetComponent<Rigidbody>().isKinematic = false;

        Debug.Log($"Disconnected [{owner.name}:{this.name}] from [{connectorToDisconnectFrom.owner.name}:{connectorToDisconnectFrom.name}]");
        connectorToDisconnectFrom.DisconnectFromAttached();
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Connector>(out Connector otherConnector))
        {
            // Debug.Log($"Is {otherConnector.owner.name} part of the core? {otherConnector.owner.isAttachedToCore()}");
            ConnectTo(otherConnector);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log($"Called OnTriggerExit on {this.name}");
        if (other.gameObject.TryGetComponent<Connector>(out Connector otherConnector))
        {
            DisconnectFromAttached();
        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!attachedConnector)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position, colliderSize);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 0.03f));
    }
}