using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RuntimeNodeEditor
{
    public class ContextMenu : MonoBehaviour
    {
        public GameObject contextContainerPrefab;
        private RectTransform _rect;
        private ContextContainer _root;

        private List<ContextContainer> _subContainers;

        public void Init()
        {
            _rect = this.GetComponent<RectTransform>();
            _subContainers = new List<ContextContainer>();

            SignalSystem.OnMenuItemClicked += OnMenuItemClicked;
        }

        private void OnMenuItemClicked(ContextMenuData data, ContextContainer container)
        {
            List<ContextContainer> toRemove = new List<ContextContainer>();
            foreach (var item in _subContainers)
            {
                if (item.depthLevel > data.Level)
                {
                    toRemove.Add(item);
                }
            }

            foreach (var item in toRemove)
            {
                Destroy(item.gameObject);
                _subContainers.Remove(item);
            }

            _subContainers.Add(container);
        }

        public void Clear()
        {
            if (_root != null)
            {
                Destroy(_root.gameObject);
                _subContainers = new List<ContextContainer>();
            }
        }

        public void Show(ContextMenuData context, Vector2 pos)
        {
            _root = Instantiate(contextContainerPrefab, _rect).GetComponent<ContextContainer>();
            _root.Init(context.children.ToArray());
            _rect.localPosition = pos;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}