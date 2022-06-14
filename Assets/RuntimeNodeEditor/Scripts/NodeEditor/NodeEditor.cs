using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class NodeEditor : MonoBehaviour
    {
        public NodeGraph                Graph  { get { return _graph; } }
        public float                    minZoom;
        public float                    maxZoom;
        public GameObject               contextMenuPrefab;
        
        private NodeGraph               _graph;
        private ContextMenu             _contextMenu;
        private ContextMenuData         _contextMenuData;

        public virtual void StartEditor(NodeGraph graph)
        {
            _graph = graph;
            _graph.Init();
            _graph.pointerListener.Init(_graph.GraphContainer, minZoom, maxZoom);
            Utility.Initialize(_graph.nodeContainer, _graph.contextMenuContainer);


            _contextMenu = Instantiate(contextMenuPrefab, _graph.contextMenuContainer).GetComponent<ContextMenu>();
            _contextMenu.Init();
            CloseContextMenu();

            _graph.pointerListener.GraphPointerClickEvent   += OnGraphPointerClick;
            _graph.pointerListener.GraphPointerDragEvent    += OnGraphPointerDrag;
            SignalSystem.NodePointerClickEvent              += OnNodePointerClick;
            SignalSystem.NodeConnectionPointerClickEvent    += OnNodeConnectionPointerClick;
        }

        public void UpdateEditor()
        {
            _graph.OnUpdate();
        }

        //  event handlers
        protected virtual void OnGraphPointerClick(PointerEventData eventData){}
        
        protected virtual void OnGraphPointerDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _graph.GraphContainer.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
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
            _graph.Save(path);
        }

        public void LoadGraph(string path)
        {
            CloseContextMenu();
            _graph.Clear();
            _graph.Load(path);
        }

        //  create graph in scene
        public TGraphComponent CreateGraph<TGraphComponent>(Transform holder) where TGraphComponent : NodeGraph
        {
            //  Create a parent
            var parent = new GameObject("NodeGraph");
            parent.transform.SetParent(holder);
            parent.AddComponent<RectTransform>().Stretch();
            parent.AddComponent<Image>();
            parent.AddComponent<Mask>();
            
            //      - add background child, stretch
            var bg = new GameObject("Background");
            bg.transform.SetParent(parent.transform);
            bg.AddComponent<RectTransform>().Stretch();
            bg.AddComponent<Image>().color = Color.gray;

            //      - add pointer listener child, stretch
            var pointerListener = new GameObject("PointerListener");
            pointerListener.transform.SetParent(parent.transform);
            pointerListener.AddComponent<RectTransform>().Stretch();
            pointerListener.AddComponent<Image>().color = Color.clear;

            //      - add graph child, center, with size
            var graph = new GameObject("Graph");
            graph.transform.SetParent(parent.transform);
            var graphRect = graph.AddComponent<RectTransform>();
            graphRect.sizeDelta = Vector2.one * 1000f;
            graphRect.anchoredPosition = Vector2.zero; 


            //          - add line container child, stretch
            var lineContainer = new GameObject("LineContainer");
            lineContainer.transform.SetParent(graph.transform);
            var lineContainerRect = lineContainer.AddComponent<RectTransform>().Stretch();
            
            //          - add node container
            var nodeContainer = new GameObject("NodeContainer");
            nodeContainer.transform.SetParent(graph.transform);
            var nodeContainerRect = nodeContainer.AddComponent<RectTransform>().Stretch();

            //              - add pointer locator 
            var pointerLocator = new GameObject("PointerLocator");
            pointerLocator.transform.SetParent(nodeContainer.transform);
            var pLocatorRect = pointerLocator.AddComponent<RectTransform>();
            pLocatorRect.sizeDelta = Vector2.zero;
            pLocatorRect.anchoredPosition = Vector2.zero;
            
            
            //      - add ctx menu child, stretch
            var ctxMenuContainer = new GameObject("CtxMenuContainer");
            ctxMenuContainer.transform.SetParent(parent.transform);
            var ctxContainerRect = ctxMenuContainer.AddComponent<RectTransform>().Stretch();

            var bezierDrawer = graph.AddComponent<BezierCurveDrawer>();
            bezierDrawer.pointerLocator = pLocatorRect;
            bezierDrawer.lineContainer = lineContainerRect;
            bezierDrawer.vertexCount = 60;
            
            var listener = pointerListener.AddComponent<GraphPointerListener>();
            
            var nodeGraph = graph.AddComponent<TGraphComponent>();
            nodeGraph.contextMenuContainer = ctxContainerRect;
            nodeGraph.nodeContainer = nodeContainerRect;
            nodeGraph.pointerListener = listener;
            nodeGraph.drawer = bezierDrawer;

            return nodeGraph;
        }
    }
}