using System;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class SignalSystem : INodeEvents, ISocketEvents, IConnectionEvents
    {
        public event Action<PointerEventData>               OnGraphPointerClickEvent;
        public event Action<PointerEventData>               OnGraphPointerDragEvent;
        public event Action<PointerEventData>               OnGraphPointerScrollEvent;

        public void InvokeGraphPointerClick(PointerEventData eventData)
        {
            OnGraphPointerClickEvent?.Invoke(eventData);
        }

        public void InvokeGraphPointerDrag(PointerEventData eventData)
        {
            OnGraphPointerDragEvent?.Invoke(eventData);
        }

        public void InvokeGraphPointerScroll(PointerEventData eventData)
        {
            OnGraphPointerScrollEvent?.Invoke(eventData);
        }

        //  INodePointerListener
        public event Action<Node, PointerEventData>         OnNodePointerClickEvent;
        public event Action<Node, PointerEventData>         OnNodePointerDownEvent;
        public event Action<Node, PointerEventData>         OnNodePointerDragEvent;
        
        public void InvokeNodePointerClick(Node node, PointerEventData eventData)
        {
            OnNodePointerClickEvent?.Invoke(node, eventData);
        }

        public void InvokeNodePointerDown(Node node, PointerEventData eventData)
        {
            OnNodePointerDownEvent?.Invoke(node, eventData);
        }

        public void InvokeNodePointerDrag(Node node, PointerEventData eventData)
        {
            OnNodePointerDragEvent?.Invoke(node, eventData);
        }
        


        //  ISocketPointerListener
        public event Action<SocketOutput>                   OnOutputSocketDragStartEvent;
        public event Action<SocketInput>                    OnOutputSocketDragDropEvent;
        public event Action<SocketInput, PointerEventData>  OnInputSocketClickEvent;
        public event Action<SocketOutput, PointerEventData> OnOutputSocketClickEvent;
        
        public void InvokeSocketDragFrom(SocketOutput output)
        {
            OnOutputSocketDragStartEvent?.Invoke(output);
        }

        public void InvokeOutputSocketDragDropTo(SocketInput input)
        {
            OnOutputSocketDragDropEvent?.Invoke(input);
        }

        public void InvokeInputSocketClick(SocketInput input, PointerEventData eventData)
        {
            OnInputSocketClickEvent?.Invoke(input, eventData);
        }

        public void InvokeOutputSocketClick(SocketOutput output, PointerEventData eventData)
        {
            OnOutputSocketClickEvent?.Invoke(output, eventData);
        }



        //  IConnectionEventListener
        public event Action<string, PointerEventData>       OnConnectionPointerClickEvent;
        public event Action<SocketInput, SocketOutput>      OnSocketConnect;
        public event Action<SocketInput, SocketOutput>      OnSocketDisconnect;
        
        public void InvokeConnectionPointerClick(string connId, PointerEventData eventData)
        {
            OnConnectionPointerClickEvent?.Invoke(connId, eventData);
        }
    
        public void InvokeSocketConnection(SocketInput input, SocketOutput output)
        {
            OnSocketConnect?.Invoke(input, output);
        }

        public void InvokeSocketDisconnection(SocketInput input, SocketOutput output)
        {
            OnSocketDisconnect?.Invoke(input, output);
        }
    }

    public interface INodeEvents
    {
        event Action<Node, PointerEventData>                OnNodePointerClickEvent;
        event Action<Node, PointerEventData>                OnNodePointerDownEvent;
        event Action<Node, PointerEventData>                OnNodePointerDragEvent;

        void InvokeNodePointerClick(Node node, PointerEventData eventData);
        void InvokeNodePointerDown(Node node, PointerEventData eventData);
        void InvokeNodePointerDrag(Node node, PointerEventData eventData);
    }

    public interface ISocketEvents
    {
        event Action<SocketOutput>                          OnOutputSocketDragStartEvent;
        event Action<SocketInput>                           OnOutputSocketDragDropEvent;
        event Action<SocketInput, PointerEventData>         OnInputSocketClickEvent;
        event Action<SocketOutput, PointerEventData>        OnOutputSocketClickEvent;

        void InvokeSocketDragFrom(SocketOutput output);
        void InvokeOutputSocketDragDropTo(SocketInput input);
        void InvokeInputSocketClick(SocketInput input, PointerEventData eventData);
        void InvokeOutputSocketClick(SocketOutput output, PointerEventData eventData);
    }

    public interface IConnectionEvents
    {
        event Action<string,      PointerEventData>         OnConnectionPointerClickEvent;
        event Action<SocketInput, SocketOutput>             OnSocketConnect;
        event Action<SocketInput, SocketOutput>             OnSocketDisconnect;

        void InvokeConnectionPointerClick(string connId, PointerEventData eventData);
        void InvokeSocketConnection(SocketInput input, SocketOutput output);
        void InvokeSocketDisconnection(SocketInput input, SocketOutput output);
    }
}