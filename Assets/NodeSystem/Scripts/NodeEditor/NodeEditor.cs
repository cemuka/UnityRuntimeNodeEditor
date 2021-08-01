using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeEditor : MonoBehaviour
{
    public float minZoom;
    public float maxZoom;
    public NodeGraph graph;
    public GraphPointerListener pointerListener;

    public RectTransform contextMenuContainer;
    public RectTransform nodeContainer;

    private ContextMenu _contextMenu;

    private void Start()
    {
        Application.targetFrameRate = 60;

        graph.Init(nodeContainer);
        pointerListener.Init(graph.GraphContainer, minZoom, maxZoom);
        Utility.Initialize(nodeContainer, contextMenuContainer);
        GraphPointerListener.GraphPointerClickEvent  += OnGraphPointerClick;
        GraphPointerListener.GraphPointerDragEvent   += OnGraphPointerDrag;
        SignalSystem.NodePointerClickEvent           += OnNodePointerClick;
        
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
            case PointerEventData.InputButton.Right:
            {
                DisplayGraphContextMenu();
            }
            break;

            case PointerEventData.InputButton.Left:
            {
                CloseContextMenu();
            }
            break;
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

    //  context methods
    private void DisplayGraphContextMenu()
    {
        _contextMenu.Clear();

        _contextMenu.AddItem("float node",           CreateFloatNode);
        _contextMenu.AddItem("operation node",      CreateMatOpNode);  

        _contextMenu.Show(Utility.GetCtxMenuPointerPosition());
    }

    private void DisplayNodeContexMenu(Node node)
    {
        _contextMenu.Clear();

        _contextMenu.AddItem("delete",                      ()=>DeleteNode(node));
        _contextMenu.AddItem("clear connections",           ()=>ClearConnections(node));
        
        _contextMenu.Show(Utility.GetCtxMenuPointerPosition());
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

    private void ClearConnections(Node node)
    {
        CloseContextMenu();
        graph.ClearConnectionsOf(node);
    }

    [UnityEngine.ContextMenu("save")]
    public void SaveGraph()
    {
        graph.Save();
    }

    [UnityEngine.ContextMenu("load")]
    public void LoadGraph()
    {
        graph.Load();
    }
}