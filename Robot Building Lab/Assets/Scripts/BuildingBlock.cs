using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: Sometimes unpredicted behaviour when attaching / detaching new building blocks
public class BuildingBlock : MonoBehaviour
{
    [SerializeField] public bool isCoreBlock = false;
    private List<Connector> activeConnections = new();  //< Connections to core

    // Start is called before the first frame update
    void Start()
    {
        GetIsAttachedToCore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetIsAttachedToCore()
    {
        return transform.root.GetComponent<BuildingBlock>().isCoreBlock;
    }

    /// <returns>
    /// Success of the operation. False if connector was already registered as active connection.
    /// </returns>
    public bool AddActiveConnection(Connector ownedConnector)
    {
        if (!activeConnections.Contains(ownedConnector))
        {
            activeConnections.Add(ownedConnector);

            transform.SetParent(ownedConnector.attachedConnector.gameObject.transform, true);
            GetComponent<Rigidbody>().isKinematic = true;
            // owner.GetComponent<Rigidbody>().useGravity = false;
            // owner.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
            return true;
        }

        return false;
    }

    /// <returns>
    /// Success of the operation. False if connector was not registered as active connection previously.
    /// </returns>
    public bool RemoveActiveConnection(Connector connector)
    {
        bool success = activeConnections.Remove(connector);
        if (activeConnections.Count == 0)
        {
            transform.SetParent(null, true);
            GetComponent<Rigidbody>().isKinematic = false;
            // owner.GetComponent<Rigidbody>().useGravity = true;
            // owner.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        return success;
    }


}
