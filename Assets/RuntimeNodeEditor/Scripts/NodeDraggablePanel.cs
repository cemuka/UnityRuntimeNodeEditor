using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class NodeDraggablePanel : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler
    {
        private Node _ownerNode;

        public void Init(Node owner)
        {
            _ownerNode = owner;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SignalSystem.InvokeNodePointerClick(_ownerNode, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SignalSystem.InvokeNodePointerDown(_ownerNode, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SignalSystem.InvokeNodePointerDrag(_ownerNode, eventData);
        }

    }
}