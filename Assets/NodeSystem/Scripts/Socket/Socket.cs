using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Socket : MonoBehaviour
{
    public IConnection connection;
    public SocketHandle handle;
    public ConnectionType connectionType;
    [HideInInspector]public Node parentNode;

    public void Init(Node parent)
    {
        this.parentNode = parent;
    }

    public bool HasConnection()
    {
        return connection != null;
    }
}