using UnityEngine;

namespace RuntimeNodeEditor
{
    public class Socket : MonoBehaviour
    {
        public Node             OwnerNode { get { return _ownerNode; } }
        public ISocketEvents    Events    { get { return _socketEvents; } }
        
        public string           socketId;
        public IConnection      connection;
        public SocketHandle     handle;
        public ConnectionType   connectionType;
        private Node            _ownerNode;
        private ISocketEvents   _socketEvents;

        public void SetOwner(Node owner, ISocketEvents events)
        {
            _ownerNode = owner;
            _socketEvents = events;
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