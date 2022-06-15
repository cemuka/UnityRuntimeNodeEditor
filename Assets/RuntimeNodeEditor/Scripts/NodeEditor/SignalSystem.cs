using System;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class SignalSystem : INodeEvents, ISocketEvents, IConnectionEvents, IContextMenuEvents
    {
        public event Action<PointerEventData> GraphPointerClickEvent;
        public event Action<PointerEventData> GraphPointerDragEvent;

        public void InvokeGraphPointerClick(PointerEventData eventData)
        {
            GraphPointerClickEvent?.Invoke(eventData);
        }

        public void InvokeGraphPointerDrag(PointerEventData eventData)
        {
            GraphPointerDragEvent?.Invoke(eventData);
        }

        //  INodePointerListener
        public event Action<Node, PointerEventData>              NodePointerClickEvent;
        public event Action<Node, PointerEventData>              NodePointerDownEvent;
        public event Action<Node, PointerEventData>              NodePointerDragEvent;
        
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
        


        //  ISocketPointerListener
        public event Action<SocketOutput>                        OutputSocketDragStartEvent;
        public event Action<SocketInput>                         OutputSocketDragDrop;
        public event Action<SocketInput, PointerEventData>       InputSocketClickEvent;
        public event Action<SocketOutput, PointerEventData>      OutputSocketClickEvent;
        
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



        //  IConnectionEventListener
        public event Action<string, PointerEventData>            NodeConnectionPointerClickEvent;
        public event Action<SocketInput, SocketOutput>           SocketConnect;
        public event Action<SocketInput, SocketOutput>           SocketDisconnect;
        
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

        //  IContextMenuListener
        public event Action<ContextMenuData, ContextContainer>   OnMenuItemClicked;

        public void InvokeMenuItemClicked(ContextMenuData data, ContextContainer container)
        {
            OnMenuItemClicked?.Invoke(data, container);
        }

    }

    public interface INodeEvents
    {
        event Action<Node, PointerEventData> NodePointerClickEvent;
        event Action<Node, PointerEventData> NodePointerDownEvent;
        event Action<Node, PointerEventData> NodePointerDragEvent;

        void InvokeNodePointerClick(Node node, PointerEventData eventData);
    }

    public interface ISocketEvents
    {
        event Action<SocketOutput>                        OutputSocketDragStartEvent;
        event Action<SocketInput>                         OutputSocketDragDrop;
        event Action<SocketInput, PointerEventData>       InputSocketClickEvent;
        event Action<SocketOutput, PointerEventData>      OutputSocketClickEvent;

        void InvokeSocketDragFrom(SocketOutput output);
        void InvokeOutputSocketDragDropTo(SocketInput input);
        void InvokeInputSocketClick(SocketInput input, PointerEventData eventData);
        void InvokeOutputSocketClick(SocketOutput output, PointerEventData eventData);
    }

    public interface IConnectionEvents
    {
        event Action<string,      PointerEventData>       NodeConnectionPointerClickEvent;
        event Action<SocketInput, SocketOutput>           SocketConnect;
        event Action<SocketInput, SocketOutput>           SocketDisconnect;

        void InvokeNodeConnectionPointerClick(string connId, PointerEventData eventData);
        void InvokeSocketConnection(SocketInput input, SocketOutput output);
        void InvokeSocketDisconnection(SocketInput input, SocketOutput output);
    }

    public interface IContextMenuEvents
    {
        event Action<ContextMenuData, ContextContainer>   OnMenuItemClicked;

        void InvokeMenuItemClicked(ContextMenuData data, ContextContainer container);
    }
}