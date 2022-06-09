using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class NodeGraph : MonoBehaviour
    {
        //  scene references
        public RectTransform        contextMenuContainer;
        public RectTransform        nodeContainer;
        public GraphPointerListener pointerListener;
        public BezierCurveDrawer    drawer;

        public List<Node>           nodes;
        public List<Connection>     connections;

        //  cache
        private SocketOutput        _currentDraggingSocket;
        private Vector2             _pointerOffset;
	    private Vector2             _localPointerPos;
	    private Vector2             _duplicateOffset;
        private RectTransform       _nodeContainer;
	    private RectTransform       _graphContainer;

        public RectTransform GraphContainer => _graphContainer;


        public void Init()
        {
            _nodeContainer      = nodeContainer;
            _graphContainer     = this.GetComponent<RectTransform>();
	        _duplicateOffset    = (Vector2.one * 10f);
            nodes               = new List<Node>();
	        connections         = new List<Connection>();

            SignalSystem.OutputSocketDragStartEvent     += OnOutputDragStarted;
            SignalSystem.OutputSocketDragDrop           += OnOutputDragDroppedTo;
            SignalSystem.InputSocketClickEvent          += OnInputSocketClicked;
            SignalSystem.OutputSocketClickEvent         += OnOutputSocketClicked;
            SignalSystem.NodePointerDownEvent           += OnNodePointerDown;
            SignalSystem.NodePointerDragEvent           += OnNodePointerDrag;

            drawer.Init();
        }

        public void Create(string prefabPath)
        {
	        var mousePosition = Vector2.zero;
#if ENABLE_LEGACY_INPUT_MANAGER
	        mousePosition = Input.mousePosition;
#endif
#if ENABLE_INPUT_SYSTEM
	        mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif
	        var pos = Utility.GetLocalPointIn(nodeContainer, mousePosition);
            var node = Utility.CreateNodePrefab<Node>(prefabPath);
            node.Init(pos, CreateId, prefabPath);
            node.Setup();
            nodes.Add(node);
            HandleSocketRegister(node);
        }

        public void Create(string prefabPath, Vector2 pos)
        {
            var node = Utility.CreateNodePrefab<Node>(prefabPath);
            node.Init(pos, CreateId, prefabPath);
            node.Setup();
            nodes.Add(node);
            HandleSocketRegister(node);
        }

        public void Delete(Node node)
        {
            ClearConnectionsOf(node);
            Destroy(node.gameObject);
            nodes.Remove(node);
        }
        
	    public void Duplicate(Node node)
	    {
		    Serializer info = new Serializer();
		    node.OnSerialize(info);
		    Create(node.Path, node.Position + _duplicateOffset);
		    var newNode = nodes.Last();
		    newNode.OnDeserialize(info);
	    }

        public void Connect(SocketInput input, SocketOutput output)
        {
            var connection = new Connection()
            {
                connId = CreateId,
                input = input,
                output = output
            };

            input.Connect(connection);
            output.Connect(connection);

            connections.Add(connection);
            input.parentNode.Connect(input, output);

            drawer.Add(connection.connId, output.handle, input.handle);
        }

        public void Disconnect(Connection conn)
        {
            drawer.Remove(conn.connId);
            conn.input.parentNode.Disconnect(conn.input, conn.output);

            conn.input.Disconnect();
            conn.output.Disconnect();

            connections.Remove(conn);
        }

        public void Disconnect(IConnection conn)
        {
            var connection = connections.FirstOrDefault<Connection>(c => c.connId == conn.ConnId);
            Disconnect(connection);
        }

        public void Disconnect(string id)
        {
            var connection = connections.FirstOrDefault<Connection>(c => c.connId == id);
            Disconnect(connection);
        }

        public void ClearConnectionsOf(Node node)
        {
            connections.Where(conn => conn.output.parentNode == node || conn.input.parentNode == node)
                .ToList()
                .ForEach(conn => Disconnect(conn));
        }

        public void Save(string path)
        {
            var graph = new GraphData();
            var nodeDatas = new List<NodeData>();
            var connDatas = new List<ConnectionData>();

            foreach (var node in nodes)
            {
                var ser = new Serializer();
                var data = new NodeData();
                node.OnSerialize(ser);

                data.id = node.ID;
                data.values = ser.Serialize();
                data.posX = node.Position.x;
                data.posY = node.Position.y;
                data.path = node.Path;

                var inputIds = new List<string>();
                foreach (var input in node.inputs)
                {
                    inputIds.Add(input.socketId);
                }

                var outputIds = new List<string>();
                foreach (var output in node.outputs)
                {
                    outputIds.Add(output.socketId);
                }

                data.inputSocketIds = inputIds.ToArray();
                data.outputSocketIds = outputIds.ToArray();

                nodeDatas.Add(data);
            }

            foreach (var conn in connections)
            {
                var data = new ConnectionData();
                data.id = conn.connId;
                data.outputSocketId = conn.output.socketId;
                data.inputSocketId = conn.input.socketId;

                connDatas.Add(data);
            }

            graph.name = "awesome graph";
            graph.nodes = nodeDatas.ToArray();
            graph.connections = connDatas.ToArray();

            System.IO.File.WriteAllText(path, JsonUtility.ToJson(graph, true));
        }

        public void Load(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var file = System.IO.File.ReadAllText(path);
                var graph = JsonUtility.FromJson<GraphData>(file);

                foreach (var data in graph.nodes)
                {
                    LoadNode(data);
                }

                foreach (var node in nodes)
                {
                    var nodeData = graph.nodes.FirstOrDefault(data => data.id == node.ID);

                    for (int i = 0; i < nodeData.inputSocketIds.Length; i++)
                    {
                        node.inputs[i].socketId = nodeData.inputSocketIds[i];
                    }

                    for (int i = 0; i < nodeData.outputSocketIds.Length; i++)
                    {
                        node.outputs[i].socketId = nodeData.outputSocketIds[i];
                    }
                }

                foreach (var data in graph.connections)
                {
                    LoadConn(data);
                }
            }
        }

        public void Clear()
        {
            var nodesToClear = new List<Node>(nodes);
            nodesToClear.ForEach(n => Delete(n));
        }

        public void OnUpdate()
        {
            pointerListener.OnUpdate();
            drawer.UpdateDraw();
        }

        //  event handlers
        private void OnInputSocketClicked(SocketInput input, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                connections.Where(conn => conn.input == input)
                            .ToList()
                            .ForEach(conn => Disconnect(conn));
            }
        }

        private void OnOutputSocketClicked(SocketOutput output, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                connections.Where(conn => conn.output == output)
                            .ToList()
                            .ForEach(conn => Disconnect(conn));
            }
        }

        private void OnOutputDragDroppedTo(SocketInput target)
        {

            if (_currentDraggingSocket == null || target == null)
            {
                _currentDraggingSocket = null;
                drawer.CancelDrag();

                return;
            }
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
            if (!node.CanMove())
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Vector2 pointerPos = ClampToWindow(eventData);
                var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_nodeContainer, pointerPos,
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
            _nodeContainer.GetWorldCorners(canvasCorners);

            var clampedX = Mathf.Clamp(rawPointerPos.x, canvasCorners[0].x, canvasCorners[2].x);
            var clampedY = Mathf.Clamp(rawPointerPos.y, canvasCorners[0].y, canvasCorners[2].y);

            var newPointerPos = new Vector2(clampedX, clampedY);
            return newPointerPos;
        }

        private void HandleSocketRegister(Node node)
        {
            foreach (var i in node.inputs)
            {
                i.socketId = CreateId;
            }

            foreach (var o in node.outputs)
            {
                o.socketId = CreateId;
            }
        }

        private void LoadNode(NodeData data)
        {
            var node = Utility.CreateNodePrefab<Node>(data.path);
            node.Init(new Vector2(data.posX, data.posY), data.id, data.path);
            node.Setup();
            nodes.Add(node);

            var ser = new Serializer();
            ser.Deserialize(data.values);
            node.OnDeserialize(ser);


        }

        private void LoadConn(ConnectionData data)
        {
            var input = nodes.SelectMany(n => n.inputs).FirstOrDefault(i => i.socketId == data.inputSocketId);
            var output = nodes.SelectMany(n => n.outputs).FirstOrDefault(o => o.socketId == data.outputSocketId);

            if (input != null && output != null)
            {
                var connection = new Connection()
                {
                    connId = data.id,
                    input = input,
                    output = output
                };

                input.Connect(connection);
                output.Connect(connection);

                connections.Add(connection);
                input.parentNode.Connect(input, output);

                drawer.Add(connection.connId, output.handle, input.handle);
            }
        }

        private string CreateId => System.Guid.NewGuid().ToString();
    }
}