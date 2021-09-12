using UnityEngine;

public class ApplicationStartup : MonoBehaviour
{
    public ExampleNodeEditor editor;

    private void Start()
    {
        Application.targetFrameRate = 60;
        editor.StartEditor();
    }

    private void Update()
    {
        editor.UpdateEditor();
    }
}