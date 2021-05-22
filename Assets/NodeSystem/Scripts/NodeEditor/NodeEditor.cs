using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeEditor : MonoBehaviour
{
    public NodeGraph graph;

    public RectTransform contextMenuContainer;
    public RectTransform nodeContainer;

    private static RectTransform _contextMenuContainer;
    private ContextMenu _contextMenu;

    private void Start()
    {
        Application.targetFrameRate = 60;

        graph.Init();
        GraphPointerListener.GraphPointerEvent  += OnGraphMouseClick;

        _contextMenuContainer = contextMenuContainer;
        _contextMenu = CreatePrefab<ContextMenu>("Prefabs/ContextMenu", _contextMenuContainer);        
        _contextMenu.Init();
        _contextMenu.Hide();
        _contextMenu.AddItem("float node",           CreateFloatNode);
        _contextMenu.AddItem("operation node",      CreateOperationNode);   
    }

    //  context actions
    private void CreateFloatNode()
    {
        _contextMenu.Hide();

        var floatNode = CreatePrefab<FloatNode>("Prefabs/Nodes/FloatNode", nodeContainer);
        var pos = GetMousePosition();
        floatNode.Init(pos);
        NodeGraph.nodes.Add(floatNode);
    }

    private void CreateOperationNode()
    {
        _contextMenu.Hide();

        var opNode = CreatePrefab<MathOperationNode>("Prefabs/Nodes/MathOperationNode", nodeContainer);
        var pos = GetMousePosition();
        opNode.Init(pos);
        NodeGraph.nodes.Add(opNode);
    }

    //  event handlers
    private void OnGraphMouseClick(PointerEventData pointerEvent)
    {
        switch (pointerEvent.button)
        {
            case PointerEventData.InputButton.Right:
            {
                _contextMenu.Show(GetMousePosition());
            }
            break;

            case PointerEventData.InputButton.Left:
            {
                _contextMenu.Hide();
            }
            break;
        }
    }

    //  helpers
    public static Vector2 GetMousePosition()
    {
        Vector2 localPointerPos;
        var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_contextMenuContainer,
                                                                              Input.mousePosition,
                                                                              null,
                                                                              out localPointerPos);
    
        return localPointerPos;
    }

    public static T CreatePrefab<T>(string path, Transform parent)
    {
        var prefab = Resources.Load<GameObject>(path);
        var instance = Instantiate(prefab, parent);
        var component = instance.GetComponent<T>();

        return component;
    }
}