using System;
using UnityEngine.EventSystems;

public static class SignalSystem
{
    public static event Action<Socket> ConnectionDragStartEvent;
    public static event Action<Socket> ConnectionDragDropEvent;

    public static void InvokeSocketDragFrom(Socket socket)
    {
        ConnectionDragStartEvent?.Invoke(socket);
    }

    public static void InvokeSocketDragDropWith(Socket socket)
    {
        ConnectionDragDropEvent?.Invoke(socket);
    }
}