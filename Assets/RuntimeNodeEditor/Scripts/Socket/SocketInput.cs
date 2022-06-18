using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class SocketInput : Socket, IPointerClickHandler
    {
        public List<Connection> Connections { get; private set; }

        public override void Setup()
        {
            Connections = new List<Connection>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Events.InvokeInputSocketClick(this, eventData);
        }

        public void Connect(Connection conn)
        {
            Connections.Add(conn);
        }

        public void Disconnect(Connection conn)
        {
            Connections.Remove(conn);
        }

        public override bool HasConnection()
        {
            return Connections.Count > 0;
        }
    }
}