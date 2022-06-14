using UnityEngine;

public class ApplicationStartup : MonoBehaviour
{
    public Transform editorHolder1;
    public ExampleNodeEditor editor1;

    private void Start()
    {
        Application.targetFrameRate = 60;
        var graph1 = editor1.CreateGraph<MyGraph>(editorHolder1);
        editor1.StartEditor(graph1);
    }

    private void Update()
    {
        editor1.UpdateEditor();
    }
}