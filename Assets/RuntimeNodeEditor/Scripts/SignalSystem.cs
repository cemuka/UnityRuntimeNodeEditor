using System;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class SignalSystem
    {
        public event Action<SocketOutput>                        OutputSocketDragStartEvent;
        public event Action<SocketInput>                         OutputSocketDragDrop;
        public event Action<SocketInput, PointerEventData>       InputSocketClickEvent;
        public event Action<SocketOutput, PointerEventData>      OutputSocketClickEvent;
        public event Action<Node, PointerEventData>              NodePointerClickEvent;
        public event Action<Node, PointerEventData>              NodePointerDownEvent;
        public event Action<Node, PointerEventData>              NodePointerDragEvent;
        public event Action<ContextMenuData, ContextContainer>   OnMenuItemClicked;
        public event Action<string, PointerEventData>            NodeConnectionPointerClickEvent;
        public event Action<SocketInput, SocketOutput>           SocketConnect;
        public event Action<SocketInput, SocketOutput>           SocketDisconnect;

        public void InvokeSocketDragFrom(SocketOutput output)
        {
            OutputSocketDragStartEvent?.Invoke(output);
        }

        public void InvokeOutputSocketDragDropTo(SocketInput input)
        {
            OutputSocketDragDrop?.Invoke(input);
        }

        public void InvokeInputSocketClick(SocketInput input, PointerEventData eventData)
        {
            InputSocketClickEvent?.Invoke(input, eventData);
        }

        public void InvokeOutputSocketClick(SocketOutput output, PointerEventData eventData)
        {
            OutputSocketClickEvent?.Invoke(output, eventData);
        }

        public void InvokeNodePointerClick(Node node, PointerEventData eventData)
        {
            NodePointerClickEvent?.Invoke(node, eventData);
        }

        public void InvokeNodePointerDown(Node node, PointerEventData eventData)
        {
            NodePointerDownEvent?.Invoke(node, eventData);
        }

        public void InvokeNodePointerDrag(Node node, PointerEventData eventData)
        {
            NodePointerDragEvent?.Invoke(node, eventData);
        }

        public void InvokeMenuItemClicked(ContextMenuData data, ContextContainer container)
        {
            OnMenuItemClicked?.Invoke(data, container);
        }

        public void InvokeNodeConnectionPointerClick(string connId, PointerEventData eventData)
        {
            NodeConnectionPointerClickEvent?.Invoke(connId, eventData);
        }
    
        public void InvokeSocketConnection(SocketInput input, SocketOutput output)
        {
            SocketConnect?.Invoke(input, output);
        }

        public void InvokeSocketDisconnection(SocketInput input, SocketOutput output)
        {
            SocketDisconnect?.Invoke(input, output);
        }
    }
}