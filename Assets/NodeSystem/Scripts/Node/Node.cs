using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public TMP_Text headerText;
    public GameObject body;
    public RectTransform PanelRect => _panelRectTransform;
    
    private NodeDraggablePanel _dragPanel;
    private NodeType _nodeType;
    private RectTransform _panelRectTransform;

    public virtual void Init(Vector2 pos)
    {
        _panelRectTransform = body.transform.parent.GetComponent<RectTransform>();
        _dragPanel = body.AddComponent<NodeDraggablePanel>();
        _dragPanel.Init(this);

        SetPosition(pos);
    }

    public virtual void OnConnection(SocketInput input, IOutput output)
    {

    }

    public void SetHeader(string name)
    {
        headerText.SetText(name);
    }

    public void SetType(NodeType type)
    {
        _nodeType = type;
    }

    public void SetPosition(Vector2 pos)
    {
        _panelRectTransform.localPosition = pos;
    }

    public void SetAsLastSibling()
    {
        _panelRectTransform.SetAsLastSibling();
    }

}