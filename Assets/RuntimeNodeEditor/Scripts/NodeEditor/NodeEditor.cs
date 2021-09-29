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
        private ContextMenuData _contextMenuData;
        private GraphPointerListener _pointerListener;

        public virtual void StartEditor()
        {
            graph.Init();
            graph.pointerListener.Init(graph.GraphContainer, minZoom, maxZoom);

            Utility.Initialize(graph.nodeContainer, graph.contextMenuContainer);

            _contextMenu = Utility.CreatePrefab<ContextMenu>("Prefabs/ContextMenu", graph.contextMenuContainer);
            _contextMenu.Init();
            CloseContextMenu();

            graph.pointerListener.GraphPointerClickEvent        += OnGraphPointerClick;
            graph.pointerListener.GraphPointerDragEvent         += OnGraphPointerDrag;
            SignalSystem.NodePointerClickEvent                  += OnNodePointerClick;
            SignalSystem.NodeConnectionPointerClickEvent         += OnNodeConnectionPointerClick;
        }

        public void UpdateEditor()
        {
            graph.OnUpdate();
        }

        //  event handlers
        protected virtual void OnGraphPointerClick(PointerEventData eventData){}
        
        protected virtual void OnGraphPointerDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                graph.GraphContainer.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
            }
        }

        protected virtual void OnNodePointerClick(Node node, PointerEventData eventData){}
        
        protected virtual void OnNodeConnectionPointerClick(string connId, PointerEventData eventData){}

        //  context methods
        protected void DisplayContextMenu()
        {
            _contextMenu.Clear();
            _contextMenu.Show(_contextMenuData, Utility.GetCtxMenuPointerPosition());
        }

        protected void CloseContextMenu()
        {
            _contextMenu.Hide();
            _contextMenu.Clear();
        }

        public void SetContextMenu(ContextMenuData ctx)
        {
            _contextMenuData = ctx;
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
            graph.Load(path);
        }
    }
}