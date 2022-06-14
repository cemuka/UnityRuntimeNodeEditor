using RuntimeNodeEditor;
using UnityEngine;

public class MyGraph : NodeGraph
{
    public override void OnConnect(SocketInput input, SocketOutput output)
    {
        Debug.Log("connected");
    }

    public override void OnDisconnect(SocketInput input, SocketOutput output)
    {
        Debug.Log("disconnected");
    }
}