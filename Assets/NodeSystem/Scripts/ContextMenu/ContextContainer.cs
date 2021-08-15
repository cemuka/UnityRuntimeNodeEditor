using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextContainer : MonoBehaviour
{
    public Transform content;
    public int depthLevel;

    private RectTransform _rect;
    private List<ContextItem> _items;
    private GameObject _menuItemPrefab;

    public void Init(ContextMenuData[] nodes)
    {
        _rect = this.GetComponent<RectTransform>();
        _items = new List<ContextItem>();
        _menuItemPrefab = Resources.Load<GameObject>("Prefabs/ContextItem");

        foreach (var node in nodes)
        {
            depthLevel = node.Level;
            var item = Instantiate(_menuItemPrefab, content).GetComponent<ContextItem>();
            item.Init(node);
        }
    }
}


