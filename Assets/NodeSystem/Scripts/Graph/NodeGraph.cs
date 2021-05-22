using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeGraph : MonoBehaviour
{
    public static List<Node> nodes;

    //  scene references
    public BezierCurveDrawer drawer;

    //  cache
    private Socket _currentDraggingSocket;

    public void Init()
    {
        nodes = new List<Node>();
        SignalSystem.ConnectionDragStartEvent   += OnConnectionDragged;
        SignalSystem.ConnectionDragDropEvent    += OnConnectionDragDropped;
        drawer.Init();
    }

    private void Update()
    {
        drawer.UpdateDraw();
    }

    //  event handlers
    private void OnConnectionDragDropped(Socket target)
    {
        if (target == null)
        {
            
        }
        else
        {
            if(target.type == SocketType.Input)
            {
                drawer.Add(_currentDraggingSocket, target);
                target.parent.OnConnection(_currentDraggingSocket.connection);
            }
        }

        _currentDraggingSocket = null;
        drawer.CancelDrag();
    }

    private void OnConnectionDragged(Socket request)
    {
        _currentDraggingSocket = request;
        drawer.StartDrag(_currentDraggingSocket);
    }

}