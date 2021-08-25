using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class ContextItem : MonoBehaviour
    {
        public TMP_Text nameText;
        public Button button;
        public Transform subContextIcon;
        public Transform subContextTransform;

        public void Init(ContextMenuData node)
        {
            nameText.text = node.name;

            bool hasSubMenu = node.children.Count > 0;
            subContextIcon.gameObject.SetActive(hasSubMenu);

            if (hasSubMenu)
            {
                button.onClick.AddListener(
                    () =>
                    {
                        var container = Utility.CreatePrefab<ContextContainer>("Prefabs/ContextContainer", subContextTransform);
                        container.Init(node.children/*[0].children*/.ToArray());
                        SignalSystem.InvokeMenuItemClicked(node, container);
                    }
                );
            }
            else
            {
                button.onClick.AddListener(
                    () =>
                    {
                        node.callback?.Invoke();
                    }
                );
            }
        }
    }
}