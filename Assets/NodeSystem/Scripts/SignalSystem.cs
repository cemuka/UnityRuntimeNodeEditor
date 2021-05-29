using System;
using UnityEngine.EventSystems;

public static class SignalSystem
{
    public static event Action<SocketOutput>    OutputSocketDragStartEvent;
    public static event Action<SocketInput>     InputSocketDropEvent;

    public static void InvokeSocketDragFrom(SocketOutput output)
    {
        OutputSocketDragStartEvent?.Invoke(output);
    }

    public static void InvokeInputSocketDropTo(SocketInput input)
    {
        InputSocketDropEvent?.Invoke(input);
    }
}