using UnityEngine;

public class Connection : IConnection
{
    public int id;
    public SocketInput      input;
    public SocketOutput     output;

    int IConnection.Id => id;
}

public interface IConnection
{
    int Id { get; }
}