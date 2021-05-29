using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public TMP_Text headerText;
    public GameObject body;
    
    private DragPanel _dragPanel;
    private object _value;
    private NodeType _nodeType;

    public virtual void Init(Vector2 pos)
    {
        body.transform.parent.GetComponent<RectTransform>().localPosition = pos;
        _dragPanel = body.AddComponent<DragPanel>();
        _dragPanel.Init();
    }

    public virtual void OnConnection(SocketInput port, IOutput output)
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
}