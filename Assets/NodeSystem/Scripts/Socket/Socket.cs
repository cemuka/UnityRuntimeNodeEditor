using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Socket : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    public Transform handle1;
    public Transform handle2;
    public SocketType type;
    public ConnectionType connectionType;
    public IConnection connection;
    [HideInInspector]public Node parent;

    public void Init(Node parent)
    {
        this.parent = parent;
        connection = parent;
        var image = gameObject.AddComponent<Image>();
        image.color = Color.clear;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var item in eventData.hovered)
        {
            var socket = item.GetComponent<Socket>();
            if (socket != null)
            {
                SignalSystem.InvokeSocketDragDropWith(socket);
                return;
            }       
        }

        SignalSystem.InvokeSocketDragDropWith(null);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (type == SocketType.Output)
        {
            SignalSystem.InvokeSocketDragFrom(this);
        }
    }
}