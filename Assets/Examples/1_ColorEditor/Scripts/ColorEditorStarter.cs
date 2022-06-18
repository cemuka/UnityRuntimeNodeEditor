using UnityEngine;

namespace RuntimeNodeEditor.Examples
{
    public class ColorEditorStarter : MonoBehaviour
    {
        public RectTransform editorHolder;
        public ColorNodeEditor colorEditor;

        private void Start()
        {   
            var graph = colorEditor.CreateGraph<NodeGraph>(editorHolder);
            colorEditor.StartEditor(graph);
        }
    }
}