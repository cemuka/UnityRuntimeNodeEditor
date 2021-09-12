using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class Socket : MonoBehaviour
    {
        public string socketId;
        public IConnection connection;
        public SocketHandle handle;
        public ConnectionType connectionType;
        [HideInInspector] public Node parentNode;

        public void Init(Node parent)
        {
            this.parentNode = parent;
        }

        public bool HasConnection()
        {
            return connection != null;
        }

        public void Connect(IConnection conn)
        {
            connection = conn;
        }

        public void Disconnect()
        {
            connection = null;
        }
    }
}