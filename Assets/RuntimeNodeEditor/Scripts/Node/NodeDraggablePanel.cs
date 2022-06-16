using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class NodeDraggablePanel : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler
    {
        private Node _ownerNode;
        private INodeEvents _events;

        public void Init(Node owner, INodeEvents events)
        {
            _ownerNode = owner;
            _events = events;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _events.InvokeNodePointerClick(_ownerNode, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _events.InvokeNodePointerDown(_ownerNode, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _events.InvokeNodePointerDrag(_ownerNode, eventData);
        }

    }
}