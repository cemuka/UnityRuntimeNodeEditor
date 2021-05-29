using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeEditor : MonoBehaviour
{
    public NodeGraph graph;

    public RectTransform contextMenuContainer;
    public RectTransform nodeContainer;

    private ContextMenu _contextMenu;

    private void Start()
    {
        Application.targetFrameRate = 60;

        graph.Init();
        var utility = new Utility(nodeContainer, contextMenuContainer);
        GraphPointerListener.GraphPointerEvent  += OnGraphPointerClick;
        SignalSystem.NodePointerClickEvent           += OnNodePointerClick;
        
        _contextMenu = Utility.CreatePrefab<ContextMenu>("Prefabs/ContextMenu", contextMenuContainer);        
        _contextMenu.Init();
        CloseContextMenu();
    }

    //  event handlers
    private void OnGraphPointerClick(PointerEventData pointerEvent)
    {
        switch (pointerEvent.button)
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

        _contextMenu.Show(Utility.GetMousePosition());
    }

    private void DisplayNodeContexMenu(Node node)
    {
        _contextMenu.Clear();

        _contextMenu.AddItem("delete",              ()=>DeleteNode(node));
        
        _contextMenu.Show(Utility.GetMousePosition());
    }

    private void CloseContextMenu()
    {
        _contextMenu.Hide();
        _contextMenu.Clear();
    }

    //  context item actions
    private void CreateFloatNode()
    {
        graph.Create<FloatNode>("Prefabs/Nodes/FloatNode", Utility.GetMousePosition());
        CloseContextMenu();
    }

    private void CreateMatOpNode()
    {
        graph.Create<MathOperationNode>("Prefabs/Nodes/MathOperationNode", Utility.GetMousePosition());
        CloseContextMenu();
    }
    
    private void DeleteNode(Node node)
    {
        CloseContextMenu();
        graph.Delete(node);
    }
}