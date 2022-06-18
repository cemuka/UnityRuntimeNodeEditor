using System;
using System.Collections;
using System.Collections.Generic;
using RuntimeNodeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeNodeEditor.Examples
{
    public class ColorNodeEditor : NodeEditor
    {
        public Button createValueButton;
        public Button createDisplayButton;

        public override void StartEditor(NodeGraph graph)
        {
            base.StartEditor(graph);

            createValueButton.onClick.AddListener(() => Graph.Create("Nodes/FloatNode", Vector2.zero));
            createDisplayButton.onClick.AddListener( () => Graph.Create("Nodes/ColorDisplay", Vector2.zero));
        }
    }
}

