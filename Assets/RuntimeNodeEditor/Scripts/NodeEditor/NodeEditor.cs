using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class NodeEditor : MonoBehaviour
    {
        public NodeGraph                Graph  { get { return _graph; } }
        public SignalSystem             Events { get { return _signalSystem; } }
        public float                    minZoom = 0.3f;
        public float                    maxZoom = 2f;
        public GameObject               contextMenuPrefab;
        
        private NodeGraph               _graph;
        private ContextMenu             _contextMenu;
        private ContextItemData         _contextMenuData;
        private SignalSystem            _signalSystem;

        public virtual void StartEditor(NodeGraph graph)
        {
            _signalSystem   = new SignalSystem();
            _graph          = graph;
            _graph.Init(_signalSystem, minZoom, maxZoom);

            if (contextMenuPrefab != null)
            {
                _contextMenu    = Instantiate(contextMenuPrefab, _graph.contextMenuContainer).GetComponent<ContextMenu>();
                _contextMenu.Init();
                CloseContextMenu();
            }
        }
        
        //  context methods
        public void DisplayContextMenu()
        {
            _contextMenu.Clear();
            _contextMenu.Show(_contextMenuData, Utility.GetCtxMenuPointerPosition(Graph.contextMenuContainer));
        }

        public void CloseContextMenu()
        {
            _contextMenu.Hide();
            _contextMenu.Clear();
        }

        public void SetContextMenu(ContextItemData ctx)
        {
            _contextMenuData = ctx;
        }

        //  create graph in scene
        public TGraphComponent CreateGraph<TGraphComponent>(RectTransform holder) where TGraphComponent : NodeGraph
        {
            return CreateGraph<TGraphComponent>(holder, Color.gray, Color.yellow);
        }
        
        public TGraphComponent CreateGraph<TGraphComponent>(RectTransform holder, Color bgColor, Color connectionColor) where TGraphComponent : NodeGraph
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
            var bgRect = bg.AddComponent<RectTransform>().Stretch();
            bg.AddComponent<Image>().color = bgColor;

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
            bezierDrawer.connectionColor = connectionColor;
            
            var listener = pointerListener.AddComponent<GraphPointerListener>();
            
            var nodeGraph = graph.AddComponent<TGraphComponent>();
            nodeGraph.contextMenuContainer = ctxContainerRect;
            nodeGraph.nodeContainer = nodeContainerRect;
            nodeGraph.background = bgRect;
            nodeGraph.pointerListener = listener;
            nodeGraph.drawer = bezierDrawer;

            return nodeGraph;
        }
    }
}