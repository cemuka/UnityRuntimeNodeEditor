using UnityEngine;

namespace RuntimeNodeEditor
{
    public class Socket : MonoBehaviour
    {
        public Node             OwnerNode { get { return _ownerNode; } }
        
        public string           socketId;
        public IConnection      connection;
        public SocketHandle     handle;
        public ConnectionType   connectionType;
        private Node            _ownerNode;

        public void SetOwner(Node owner)
        {
            _ownerNode = owner;
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