using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeNodeEditor
{
    public class ContextContainer : MonoBehaviour
    {
        public Transform content;
        public GameObject menuItemPrefab;
        public int depthLevel;

        private RectTransform _rect;
        private List<ContextItem> _items;

        public void Init(ContextMenuData[] nodes)
        {
            _rect = this.GetComponent<RectTransform>();
            _items = new List<ContextItem>();

            foreach (var node in nodes)
            {
                depthLevel = node.Level;
                var item = Instantiate(menuItemPrefab, content).GetComponent<ContextItem>();
                item.Init(node);
            }
        }
    }
}


