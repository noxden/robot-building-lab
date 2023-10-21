using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    private static Vector3 colliderSize = new Vector3(0.2f, 0.2f, 0.2f);
    private new BoxCollider collider = null;
    [SerializeField] private BuildingBlock owner;
    [SerializeField] private Connector attachedConnector = null;

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

        if (attachedConnector.owner.isAttachedToCore())
        {
            owner.transform.SetParent(attachedConnector.gameObject.transform, true);
            owner.GetComponent<Rigidbody>().isKinematic = true;
            // owner.GetComponent<Rigidbody>().useGravity = false;
            // owner.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void DisconnectFromAttached()
    {
        if (attachedConnector == null)
            return;

        owner.transform.SetParent(null, true);
        owner.GetComponent<Rigidbody>().isKinematic = false;
        // owner.GetComponent<Rigidbody>().useGravity = true;
        // owner.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Connector connectorToDisconnectFrom = attachedConnector;
        attachedConnector = null;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, colliderSize);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 0.3f));
    }
}