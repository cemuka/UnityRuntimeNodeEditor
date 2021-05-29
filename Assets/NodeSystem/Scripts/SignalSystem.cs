using System;
using UnityEngine.EventSystems;

public static class SignalSystem
{
    public static event Action<SocketOutput>                OutputSocketDragStartEvent;
    public static event Action<SocketInput>                 InputSocketDropEvent;
    public static event Action<Node, PointerEventData>      NodePointerClickEvent;
    public static event Action<Node, PointerEventData>      NodePointerDownEvent;
    public static event Action<Node, PointerEventData>      NodePointerDragEvent;

    public static void InvokeSocketDragFrom(SocketOutput output)
    {
        OutputSocketDragStartEvent?.Invoke(output);
    }

    public static void InvokeInputSocketDropTo(SocketInput input)
    {
        InputSocketDropEvent?.Invoke(input);
    }

    public static void InvokeNodePointerClick(Node node, PointerEventData eventData)
    {
        NodePointerClickEvent?.Invoke(node, eventData);
    }
    
    public static void InvokeNodePointerDown(Node node, PointerEventData eventData)
    {
        NodePointerDownEvent?.Invoke(node, eventData);
    }
    
    public static void InvokeNodePointerDrag(Node node, PointerEventData eventData)
    {
        NodePointerDragEvent?.Invoke(node, eventData);
    }
}