using System;
using UnityEngine.EventSystems;

public static class SignalSystem
{
    public static event Action<Socket> RequestStartedEvent;
    public static event Action<Socket> RequestSuccesEvent;
    public static event Action RequestFailEvent;

    public static void InvokeRequestConnFrom(Socket socket)
    {
        RequestStartedEvent?.Invoke(socket);
    }

    public static void InvokeRequestSuccesWith(Socket socket)
    {
        RequestSuccesEvent?.Invoke(socket);
    }

    public static void InvokeRequestFailed()
    {
        RequestFailEvent?.Invoke();
    }
}