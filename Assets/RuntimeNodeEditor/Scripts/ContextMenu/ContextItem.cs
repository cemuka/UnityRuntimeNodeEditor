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
        public GameObject contextContainerPrefab;

        public void Init(ContextMenuData node)
        {
            nameText.text = node.name;

            bool hasSubMenu = node.children.Count > 0;
            subContextIcon.gameObject.SetActive(hasSubMenu);

            if (hasSubMenu)
            {
                button.onClick.AddListener(() =>{
                    var container = Instantiate(contextContainerPrefab, subContextTransform).GetComponent<ContextContainer>();

                    container.Init(node.children.ToArray());
                    SignalSystem.InvokeMenuItemClicked(node, container);
                });
            }
            else
            {
                button.onClick.AddListener(() =>{
                    node.callback?.Invoke();
                });
            }
        }
    }
}