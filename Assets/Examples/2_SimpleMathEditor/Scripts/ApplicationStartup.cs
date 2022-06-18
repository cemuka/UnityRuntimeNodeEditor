using UnityEngine;
using RuntimeNodeEditor;

namespace RuntimeNodeEditor.Examples
{
    public class ApplicationStartup : MonoBehaviour
    {
        public RectTransform        editorHolder;
        public ExampleNodeEditor    editor;

        private void Start()
        {
            Application.targetFrameRate = 60;
            var graph = editor.CreateGraph<NodeGraph>(editorHolder);
            editor.StartEditor(graph);
        }
    }
}