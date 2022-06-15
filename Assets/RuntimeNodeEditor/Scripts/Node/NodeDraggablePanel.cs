using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class NodeDraggablePanel : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler
    {
        private Node _ownerNode;
        private INodeEvents _signal;

        public void Init(Node owner, INodeEvents signal)
        {
            _ownerNode = owner;
            _signal = signal;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _signal.InvokeNodePointerClick(_ownerNode, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _signal.InvokeNodePointerDown(_ownerNode, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _signal.InvokeNodePointerDrag(_ownerNode, eventData);
        }

    }
}