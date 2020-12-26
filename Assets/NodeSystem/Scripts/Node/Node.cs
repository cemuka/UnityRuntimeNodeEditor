using System;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour, IConnection
{
    public GameObject body;
    
    private DragPanel _dragPanel;
    private object _value;
    private NodeType _nodeType;

    public event Action ValueUpdated;

    public virtual void Init(Vector2 pos)
    {
        body.transform.parent.GetComponent<RectTransform>().localPosition = pos;
        _dragPanel = body.AddComponent<DragPanel>();
        _dragPanel.Init();
    }

    public virtual void OnConnection(IConnection conn)
    {

    }

    public void SetValue(object value)
    {
        _value = value;
    }

    public void SetType(NodeType type)
    {
        _nodeType = type;
    }

    public void ValueUpdatedEventInvoke()
    {
        ValueUpdated?.Invoke();
    }

    public T GetValue<T>()
    {
        return (T)_value;
    }

    NodeType IConnection.GetType()
    {
        return _nodeType;
    }
}