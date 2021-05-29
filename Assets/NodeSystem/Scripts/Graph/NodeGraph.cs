using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeGraph : MonoBehaviour
{
    public List<Node> nodes;
    public List<Connection> connections;

    //  scene references
    public BezierCurveDrawer drawer;

    //  cache
    private SocketOutput _currentDraggingSocket;
    private Vector2 _pointerOffset;
    private Vector2 _localPointerPos;
    private RectTransform _container;

    public void Init()
    {
        _container      = this.GetComponent<RectTransform>();
        nodes           = new List<Node>();
        connections     = new List<Connection>();

        SignalSystem.OutputSocketDragStartEvent     += OnOutputDragStarted;
        SignalSystem.InputSocketDropEvent           += OnInputDropped;
        SignalSystem.NodePointerDownEvent           += OnNodePointerDown;
        SignalSystem.NodePointerDragEvent           += OnNodePointerDrag;

        drawer.Init();
    }

    public void Create<T>(string prefabPath, Vector2 pos) where T : Node
    {
        var node = Utility.CreateNodePrefab<T>(prefabPath);
        node.Init(pos);
        nodes.Add(node);
    }

    public void Delete(Node node)
    {
        Destroy(node.gameObject);
        nodes.Remove(node);
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

    private void OnNodePointerDown(Node node, PointerEventData eventData)
    {
        node.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(node.PanelRect, eventData.position, 
                                                                eventData.pressEventCamera, out _pointerOffset);
        HandleNodeDragEvent(node, eventData);
    }

    private void OnNodePointerDrag(Node node, PointerEventData eventData)
    {
        HandleNodeDragEvent(node, eventData);
    }


    //  helper methods
    private void HandleNodeDragEvent(Node node, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 pointerPos = ClampToWindow(eventData);
            var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_container, Input.mousePosition,
                                                                            eventData.pressEventCamera, out _localPointerPos);
            if (success)
            {
                node.SetPosition(_localPointerPos - _pointerOffset);
            }
        }
    }

    private Vector2 ClampToWindow(PointerEventData eventData)
    {
        var rawPointerPos = eventData.position;
        var canvasCorners = new Vector3[4];
        _container.GetWorldCorners(canvasCorners);

        var clampedX = Mathf.Clamp(rawPointerPos.x, canvasCorners[0].x, canvasCorners[2].x);
        var clampedY = Mathf.Clamp(rawPointerPos.y, canvasCorners[0].y, canvasCorners[2].y);

        var newPointerPos = new Vector2(clampedX, clampedY);
        return newPointerPos;
    }
}