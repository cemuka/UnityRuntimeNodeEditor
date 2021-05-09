using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeGraph : MonoBehaviour
{
    [HideInInspector] public List<Node> nodes;

    //  scene references
    public BezierCurveDrawer drawer;
    public RectTransform contextMenuContainer;
    public RectTransform nodeContainer;

    //  context menu
    private ContextMenu _contextMenu;

    //  cache
    private Socket _requestSocket;

    private void Start()
    {
        drawer.Init();

        GraphPointerListener.GraphPointerEvent  += OnGraphMouseClick;
        
        SignalSystem.RequestStartedEvent        += OnRequestStarted;
        SignalSystem.RequestSuccesEvent         += OnRequestSuccess;
        SignalSystem.RequestFailEvent           += OnRequestFailed;

        _contextMenu = CreatePrefab<ContextMenu>("Prefabs/ContextMenu", contextMenuContainer);
        
        _contextMenu.Init();
        _contextMenu.Hide();
        _contextMenu.AddItem("float node",               OnContextItemSelected<FloatNode>        ("Prefabs/Nodes/FloatNode"));
        _contextMenu.AddItem("operation node",          OnContextItemSelected<MathOperationNode>("Prefabs/Nodes/MathOperationNode"));
    }

    private void Update()
    {
        drawer.UpdateDraw();
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

    private void OnRequestFailed()
    {
        drawer.CancelRequest();
        _requestSocket = null;
    }

    private void OnRequestSuccess(Socket target)
    {
        if(target.type == SocketType.Input)
        {
            drawer.Add(_requestSocket, target);
            target.parent.OnConnection(_requestSocket.connection);
        }

        _requestSocket = null;
        drawer.CancelRequest();
    }

    private void OnRequestStarted(Socket request)
    {
        _requestSocket = request;
        drawer.DrawRequest(_requestSocket);
    }

    private Action OnContextItemSelected<T>(string path) where T : Node
    {
        Action callback = null;

        callback = () => 
        { 
            _contextMenu.Hide();
            var node = CreatePrefab<T>(path, nodeContainer);
            var pos = GetMousePosition();
            node.Init(pos);
        }; 

        return callback; 
    }


    //  helper
    private Vector2 GetMousePosition()
    {
        Vector2 localPointerPos;
        var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(nodeContainer,
                                                                              Input.mousePosition,
                                                                              null,
                                                                              out localPointerPos);
    
        return localPointerPos;
    }

    public static GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }

    public static T CreatePrefab<T>(string path, Transform parent)
    {
        var prefab = Resources.Load<GameObject>(path);
        var instance = Instantiate(prefab, parent);
        var component = instance.GetComponent<T>();

        return component;
    }
}