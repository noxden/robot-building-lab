using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: Multi-Path Connections to the core are still not possible
public class Connector : MonoBehaviour
{
    private static Vector3 colliderSize = new Vector3(0.02f, 0.02f, 0.02f);
    private new BoxCollider collider = null;
    [SerializeField] private BuildingBlock owner;
    [SerializeField] public Connector attachedConnector = null;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<BuildingBlock>();
        CreateCollider();
    }

    // Update is called once per frame
    void Update()
    {
        collider.size = colliderSize;
    }

    private void CreateCollider()
    {
        collider = GetComponent<BoxCollider>();
        if (!collider)
            collider = gameObject.AddComponent<BoxCollider>();

        collider.isTrigger = true;
    }

    private void ConnectTo(Connector otherConnector)
    {
        attachedConnector = otherConnector;
        Debug.Log($"Connected [{owner.name}:{this.name}] to [{otherConnector.owner.name}:{otherConnector.name}]");

        if (attachedConnector.owner.GetIsAttachedToCore())
        {
            owner.AddActiveConnection(this);
        }
    }

    private void DisconnectFromAttached()
    {
        if (attachedConnector == null)
            return;

        Connector connectorToDisconnectFrom = attachedConnector;
        attachedConnector = null;
        owner.RemoveActiveConnection(this);
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
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 0.3f * transform.localScale.magnitude));
    }
}