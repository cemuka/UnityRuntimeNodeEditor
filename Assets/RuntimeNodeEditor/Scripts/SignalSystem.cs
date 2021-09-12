using System;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public static class SignalSystem
    {
        public static event Action<SocketOutput> OutputSocketDragStartEvent;
        public static event Action<SocketInput> OutputSocketDragDrop;
        public static event Action<SocketInput, PointerEventData> InputSocketClickEvent;
        public static event Action<SocketOutput, PointerEventData> OutputSocketClickEvent;
        public static event Action<Node, PointerEventData> NodePointerClickEvent;
        public static event Action<Node, PointerEventData> NodePointerDownEvent;
        public static event Action<Node, PointerEventData> NodePointerDragEvent;
        public static event Action<ContextMenuData, ContextContainer> OnMenuItemClicked;
        public static event Action<string, PointerEventData> NodeConnectionPointerDownEvent;

        public static void InvokeSocketDragFrom(SocketOutput output)
        {
            OutputSocketDragStartEvent?.Invoke(output);
        }

        public static void InvokeOutputSocketDragDropTo(SocketInput input)
        {
            OutputSocketDragDrop?.Invoke(input);
        }

        public static void InvokeInputSocketClick(SocketInput input, PointerEventData eventData)
        {
            InputSocketClickEvent?.Invoke(input, eventData);
        }

        public static void InvokeOutputSocketClick(SocketOutput output, PointerEventData eventData)
        {
            OutputSocketClickEvent?.Invoke(output, eventData);
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

        public static void InvokeMenuItemClicked(ContextMenuData data, ContextContainer container)
        {
            OnMenuItemClicked?.Invoke(data, container);
        }

        public static void InvokeNodeConnectionPointerDown(string connId, PointerEventData eventData)
        {
            NodeConnectionPointerDownEvent?.Invoke(connId, eventData);
        }
    }
}