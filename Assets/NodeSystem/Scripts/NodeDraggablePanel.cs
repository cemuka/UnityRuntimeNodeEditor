using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeDraggablePanel : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler
{
    private Node _parentNode;

    public void Init(Node parent)
    {
        _parentNode = parent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SignalSystem.InvokeNodePointerClick(_parentNode, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SignalSystem.InvokeNodePointerDown(_parentNode, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SignalSystem.InvokeNodePointerDrag(_parentNode, eventData);
    }

}
