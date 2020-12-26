using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public Transform content;

    private RectTransform _rect;
    private List<ContextItem> _items;

    public void Init()
    {
        _rect = this.GetComponent<RectTransform>();
        _items = new List<ContextItem>();
    }

    public void Clear()
    {
        _items.ForEach(item => Destroy(item.gameObject));
        _items.Clear();
    }

    public void AddItem(string name, Action callback)
    {
        var item = NodeGraph.CreatePrefab<ContextItem>("Prefabs/ContextItem", content);

        item.nameText.text = name;
        item.button.onClick.AddListener(() => callback());

        _items.Add(item);
    }

    public void Show(Vector2 pos)
    {
        _rect.localPosition = pos;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
