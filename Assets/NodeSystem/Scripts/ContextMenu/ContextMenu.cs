using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public Transform content;

    private RectTransform _rect;
    private List<ContextItem> _items;
    private GameObject _menuItemPrefab;

    public void Init()
    {
        _rect = this.GetComponent<RectTransform>();
        _items = new List<ContextItem>();
        _menuItemPrefab = Resources.Load<GameObject>("Prefabs/ContextItem");
    }

    public void Clear()
    {
        _items.ForEach(item => Destroy(item.gameObject));
        _items.Clear();
    }

    public void AddItem(string name, Action callback)
    {
        var item = Instantiate(_menuItemPrefab, content).GetComponent<ContextItem>();
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
