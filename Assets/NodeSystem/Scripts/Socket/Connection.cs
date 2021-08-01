using UnityEngine;

public class Connection : IConnection
{
    public string connId;
    public SocketInput      input;
    public SocketOutput     output;

    string IConnection.ConnId => connId;
}

public interface IConnection
{
    string ConnId { get; }
}