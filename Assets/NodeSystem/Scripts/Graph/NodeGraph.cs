using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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
        _maxId = 0;

        _container      = this.GetComponent<RectTransform>();
        nodes           = new List<Node>();
        connections     = new List<Connection>();

        SignalSystem.OutputSocketDragStartEvent     += OnOutputDragStarted;
        SignalSystem.OutputSocketDragDrop           += OnOutputDragDroppedTo;
        SignalSystem.InputSocketClickEvent          += OnInputSocketClicked;
        SignalSystem.OutputSocketClickEvent         += OnOutputSocketClicked;
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
        ClearConnectionsOf(node);
        Destroy(node.gameObject);
        nodes.Remove(node);
    }

    public void Connect(SocketInput input, SocketOutput output)
    {
        var connection = new Connection()
        {
            id      = CreateId,
            input   = input,
            output  = output
        };

        input.Connect(connection);
        output.Connect(connection);

        connections.Add(connection);
        input.parentNode.OnConnection(input , output);  
        
        drawer.Add(connection.id, output.handle, input.handle);
    }

    public void Disconnect(Connection conn)
    {
        drawer.Remove(conn.id);
        conn.input.parentNode.OnDisconnect(conn.input, conn.output);

        conn.input.Disconnect();
        conn.output.Disconnect();

        connections.Remove(conn);
    }

    public void Disconnect(IConnection conn)
    {
        var connection = connections.FirstOrDefault<Connection>(c => c.id == conn.Id);
        Disconnect(connection);
    }

    public void ClearConnectionsOf(Node node)
    {
        connections.Where( conn => conn.output.parentNode == node || conn.input.parentNode == node)
            .ToList()
            .ForEach(conn => Disconnect(conn));
    }

    private void Update()
    {
        drawer.UpdateDraw();
    }

    //  event handlers
    private void OnInputSocketClicked(SocketInput input, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            connections.Where( conn => conn.input == input)
                        .ToList()
                        .ForEach(conn => Disconnect(conn));
        }
    }

    private void OnOutputSocketClicked(SocketOutput output, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            connections.Where( conn => conn.output == output)
                        .ToList()
                        .ForEach(conn => Disconnect(conn));
        }
    }

    private void OnOutputDragDroppedTo(SocketInput target)  
    {
        //  if sockets connected already
        //  do nothing
        if (_currentDraggingSocket.HasConnection() && target.HasConnection())
        {
            if (_currentDraggingSocket.connection == target.connection)
            {
                _currentDraggingSocket = null;
                drawer.CancelDrag();
                
                return;
            }
        }

        if (target != null)
        {
            //  check if input allows multiple connection
            if (target.HasConnection())
            {
                //  disconnect old connection
                if (target.connectionType != ConnectionType.Multiple)
                {
                    Disconnect(target.connection);
                }
            }

            Connect(target, _currentDraggingSocket);
        }

        _currentDraggingSocket = null;
        drawer.CancelDrag();
    }

    private void OnOutputDragStarted(SocketOutput socketOnDrag)
    {
        _currentDraggingSocket = socketOnDrag;
        drawer.StartDrag(_currentDraggingSocket);

        //  check socket connection type
        if (_currentDraggingSocket.HasConnection())
        {
            //  if single, disconnect
            if (_currentDraggingSocket.connectionType == ConnectionType.Single)
            {
                Disconnect(_currentDraggingSocket.connection);
            }
        }
    }

    private void OnNodePointerDown(Node node, PointerEventData eventData)
    {
        node.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(node.PanelRect, eventData.position, 
                                                                eventData.pressEventCamera, out _pointerOffset);
        DragNode(node, eventData);
    }

    private void OnNodePointerDrag(Node node, PointerEventData eventData)
    {
        DragNode(node, eventData);
    }


    //  helper methods
    private void DragNode(Node node, PointerEventData eventData)
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

    private int _maxId;
    private int CreateId => _maxId++;
}