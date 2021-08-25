using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class NodeEditor : MonoBehaviour
    {
        public float minZoom;
        public float maxZoom;
        public NodeGraph graph;
        public GraphPointerListener pointerListener;

        public RectTransform contextMenuContainer;
        public RectTransform nodeContainer;

        private ContextMenu _contextMenu;
        private ContextMenuData _graphCtx;
        private ContextMenuData _nodeCtx;

        private void Start()
        {
            Application.targetFrameRate = 60;

            graph.Init(nodeContainer);
            pointerListener.Init(graph.GraphContainer, minZoom, maxZoom);
            Utility.Initialize(nodeContainer, contextMenuContainer);
            GraphPointerListener.GraphPointerClickEvent += OnGraphPointerClick;
            GraphPointerListener.GraphPointerDragEvent += OnGraphPointerDrag;
            SignalSystem.NodePointerClickEvent += OnNodePointerClick;
            SignalSystem.LineDownEvent += OnLineDown;

            _contextMenu = Utility.CreatePrefab<ContextMenu>("Prefabs/ContextMenu", contextMenuContainer);
            _contextMenu.Init();
            CloseContextMenu();
        }

        private void Update()
        {
            pointerListener.OnUpdate();
            graph.OnUpdate();
        }

        //  event handlers
        private void OnGraphPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right: DisplayGraphContextMenu(); break;
                case PointerEventData.InputButton.Left: CloseContextMenu(); break;
            }
        }

        private void OnGraphPointerDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                graph.GraphContainer.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
            }
        }

        private void OnNodePointerClick(Node node, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                DisplayNodeContexMenu(node);
            }
        }

        //link
        private void OnLineDown(string connId, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                DisplayLineContexMenu(connId);
            }
        }

        //  context methods
        private void DisplayGraphContextMenu()
        {
            _graphCtx = new ContextMenuBuilder()
                        .Add("nodes/float", CreateFloatNode)
                        .Add("nodes/math op",       CreateMatOpNode)
                        .Add("graph/load",          LoadGraph)
                        .Add("graph/save",          SaveGraph)
                        .Build();

            _contextMenu.Clear();
            _contextMenu.Show(_graphCtx, Utility.GetCtxMenuPointerPosition());
        }

        private void DisplayLineContexMenu(string connId)
        {
            _nodeCtx = new ContextMenuBuilder()
                .Add("delete that line", () => DisconnectConnection(connId))
                .Build();

            _contextMenu.Clear();
            _contextMenu.Show(_nodeCtx, Utility.GetCtxMenuPointerPosition());
        }

        private void DisplayNodeContexMenu(Node node)
        {
            _nodeCtx = new ContextMenuBuilder()
                .Add("clear connections", () => ClearConnections(node))
                .Add("delete", () => DeleteNode(node))
                .Build();

            _contextMenu.Clear();
            _contextMenu.Show(_nodeCtx, Utility.GetCtxMenuPointerPosition());
        }

        private void CloseContextMenu()
        {
            _contextMenu.Hide();
            _contextMenu.Clear();
        }

        //  context item actions
        private void CreateFloatNode()
        {
            var pos = Utility.GetLocalPointIn(nodeContainer, Input.mousePosition);
            graph.Create("Prefabs/Nodes/FloatNode", pos);
            CloseContextMenu();
        }

        private void CreateGroup()
        {
            var pos = Utility.GetLocalPointIn(nodeContainer, Input.mousePosition);
            graph.Create("Prefabs/Nodes/ResizeNode", pos);
            CloseContextMenu();
        }

        private void CreateMatOpNode()
        {
            var pos = Utility.GetLocalPointIn(nodeContainer, Input.mousePosition);
            graph.Create("Prefabs/Nodes/MathOperationNode", pos);
            CloseContextMenu();
        }

        private void DeleteNode(Node node)
        {
            CloseContextMenu();
            graph.Delete(node);
        }

        private void DisconnectConnection(string line_id)
        {
            CloseContextMenu();
            graph.Disconnect(line_id);
        }

        private void ClearConnections(Node node)
        {
            CloseContextMenu();
            graph.ClearConnectionsOf(node);
        }

        public void SaveGraph()
        {
            CloseContextMenu();
            graph.Save();
        }

        public void LoadGraph()
        {
            CloseContextMenu();
            graph.Clear();
            graph.Load(Application.dataPath + "/NodeSystem/Resources/Graphs/graph.json");
        }
    }
}