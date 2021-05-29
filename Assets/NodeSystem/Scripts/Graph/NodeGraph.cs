using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeGraph : MonoBehaviour
{
    public static List<Node> nodes;
    public static List<Connection> connections;

    //  scene references
    public BezierCurveDrawer drawer;

    //  cache
    private SocketOutput _currentDraggingSocket;

    public void Init()
    {
        nodes           = new List<Node>();
        connections     = new List<Connection>();

        SignalSystem.OutputSocketDragStartEvent     += OnOutputDragStarted;
        SignalSystem.InputSocketDropEvent           += OnInputDropped;

        drawer.Init();
    }

    private void Update()
    {
        drawer.UpdateDraw();
    }

    //  event handlers
    private void OnInputDropped(SocketInput target)
    {
        if (target == null)
        {
            
        }
        else
        {
            drawer.Add(_currentDraggingSocket, target);
            var connection = new Connection()
            {
                input   = target,
                output  = _currentDraggingSocket
            };
            connections.Add(connection);
            target.parentNode.OnConnection(target , _currentDraggingSocket);   
        }

        _currentDraggingSocket = null;
        drawer.CancelDrag();
    }

    private void OnOutputDragStarted(SocketOutput socketOnDrag)
    {
        _currentDraggingSocket = socketOnDrag;
        drawer.StartDrag(_currentDraggingSocket);
    }

}