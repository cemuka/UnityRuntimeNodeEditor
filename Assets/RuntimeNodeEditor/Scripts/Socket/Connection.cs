using UnityEngine;

namespace RuntimeNodeEditor
{
    public class Connection 
    {
        public readonly string connId;
        public readonly SocketInput input;
        public readonly SocketOutput output;

        public Connection(string connId, SocketInput input, SocketOutput output)
        {
            this.connId = connId;
            this.input = input;
            this.output = output;
        }
    }
}