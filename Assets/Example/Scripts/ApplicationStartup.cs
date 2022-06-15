using UnityEngine;
using RuntimeNodeEditor;

public class ApplicationStartup : MonoBehaviour
{
    public Transform editorHolder1;
    public Transform editorHolder2;
    public ExampleNodeEditor editor1;
    public ExampleNodeEditor editor2;

    private void Start()
    {
        Application.targetFrameRate = 60;
        var graph1 = editor1.CreateGraph<NodeGraph>(editorHolder1);
        editor1.StartEditor(graph1);

        var graph2 = editor2.CreateGraph<NodeGraph>(editorHolder2);
        editor2.StartEditor(graph2);
    }

    private void Update()
    {
        editor1.UpdateEditor();
        editor2.UpdateEditor();
    }
}