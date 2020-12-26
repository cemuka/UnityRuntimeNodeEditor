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
            if (socket != null && socket.type == SocketType.Input)
            {
                SignalSystem.InvokeRequestSuccesWith(socket);
                return;
            }       
        }

        SignalSystem.InvokeRequestFailed();
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (type == SocketType.Output)
        {
            SignalSystem.InvokeRequestConnFrom(this);
        }
    }
}

public enum SocketType
{
    Input,
    Output
}
