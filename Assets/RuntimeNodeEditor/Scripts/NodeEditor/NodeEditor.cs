using System;
using System.Collections.Generic;
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
        
        private ContextMenu _contextMenu;
        private ContextMenuData _graphCtx;
        private ContextMenuData _nodeCtx;
        private GraphPointerListener _pointerListener;

        private Dictionary<string, Action> _graphContextItems;
        private Dictionary<string, Action> _nodeContextItems;
        private Dictionary<string, Action> _connectionContextItems;

        public virtual void StartEditor()
        {
            graph.Init(graph.nodeContainer);
            
            _pointerListener = graph.pointerListener;
            _pointerListener.Init(graph.GraphContainer, minZoom, maxZoom);

            Utility.Initialize(graph.nodeContainer, graph.contextMenuContainer);

            _contextMenu = Utility.CreatePrefab<ContextMenu>("Prefabs/ContextMenu", graph.contextMenuContainer);
            _contextMenu.Init();
            CloseContextMenu();

            _graphContextItems      = new Dictionary<string, Action>();
            _nodeContextItems       = new Dictionary<string, Action>();
            _connectionContextItems = new Dictionary<string, Action>();

            GraphPointerListener.GraphPointerClickEvent     += OnGraphPointerClick;
            GraphPointerListener.GraphPointerDragEvent      += OnGraphPointerDrag;
            SignalSystem.NodePointerClickEvent              += OnNodePointerClick;
            SignalSystem.NodeConnectionPointerDownEvent     += OnNodeConnectionPointerDown;
        }

        public void UpdateEditor()
        {
            graph.OnUpdate();
        }

        //  event handlers
        protected virtual void OnGraphPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right: DisplayGraphContextMenu(); break;
                case PointerEventData.InputButton.Left: CloseContextMenu(); break;
            }
        }

        protected virtual void OnGraphPointerDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                graph.GraphContainer.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
            }
        }

        protected virtual void OnNodePointerClick(Node node, PointerEventData eventData){}
        protected virtual void OnNodeConnectionPointerDown(string connId, PointerEventData eventData){}

        //  context methods
        protected void DisplayGraphContextMenu()
        {
            var builder = new ContextMenuBuilder();

            foreach (var kvp in _nodeContextItems)
            {
                builder.Add(kvp.Key, kvp.Value);
            }

            _graphCtx = builder.Build();
            _contextMenu.Clear();
            _contextMenu.Show(_graphCtx, Utility.GetCtxMenuPointerPosition());
        }

        protected void DisplayNodeContexMenu(Node node)
        {
            var builder = new ContextMenuBuilder();

            foreach (var kvp in _nodeContextItems)
            {
                builder.Add(kvp.Key, kvp.Value);
            }

            _nodeCtx = builder.Build();
            _contextMenu.Clear();
            _contextMenu.Show(_nodeCtx, Utility.GetCtxMenuPointerPosition());
        }

        protected void DisplayConnectionContexMenu(string connId)
        {
            var builder = new ContextMenuBuilder();

            foreach (var kvp in _nodeContextItems)
            {
                builder.Add(kvp.Key, kvp.Value);
            }

            _nodeCtx = builder.Build();
            _contextMenu.Clear();
            _contextMenu.Show(_nodeCtx, Utility.GetCtxMenuPointerPosition());
        }

        protected void CloseContextMenu()
        {
            _contextMenu.Hide();
            _contextMenu.Clear();
        }

        public void AddGraphContextMenuItem(string path, Action onClick)
        {
            _graphContextItems.Add(path, onClick);
        }

        public void AddNodeContextMenuItem(string path, Action onClick)
        {
            _nodeContextItems.Add(path, onClick);
        }

        public void AddConnectionContextMenuItem(string path, Action onClick)
        {
            _connectionContextItems.Add(path, onClick);
        }

        //  save and load
        public void SaveGraph(string path)
        {
            CloseContextMenu();
            graph.Save(path);
        }

        public void LoadGraph(string path)
        {
            CloseContextMenu();
            graph.Clear();
            graph.Load(Application.dataPath + "/NodeSystem/Resources/Graphs/graph.json");
        }
    }
}