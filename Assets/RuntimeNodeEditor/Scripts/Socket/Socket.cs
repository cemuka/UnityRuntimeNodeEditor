using UnityEngine;

namespace RuntimeNodeEditor
{
    public class Socket : MonoBehaviour
    {
        public Node             OwnerNode { get { return _ownerNode; } }
        public ISocketEvents    Signal    { get { return _signal; } }
        
        public string           socketId;
        public IConnection      connection;
        public SocketHandle     handle;
        public ConnectionType   connectionType;
        private Node            _ownerNode;
        private ISocketEvents   _signal;

        public void SetOwner(Node owner, ISocketEvents signal)
        {
            _ownerNode = owner;
            _signal = signal;
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