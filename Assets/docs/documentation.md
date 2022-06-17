
Welcome to the RuntimeNodeEditor documentation.

## Your first editor

```c#
public class MyNodeEditor : NodeEditor
{
    public override void StartEditor(NodeGraph graph)
    {
        base.StartEditor(graph);

        //  make your custom initialization here
    }
}
```

```c#
public class ApplicationStartup : MonoBehaviour
{
    public RectTransform    editorHolder;  // graph container will stretch in this transform. 
    public MyNodeEditor     editor;

    private void Start()
    {
        var graph = editor.CreateGraph<NodeGraph>(editorHolder);
        editor.StartEditor(graph);
    }

    private void Update()
    {
        //  handles drawing connections
        editor.UpdateEditor();
    }
}
```

## Context menu
RuntimeNodeEditor comes with a simple context menu solution.
Context menu is totally optional but I believe that a node editor wouldn't be much enjoyable without a context menu. 
Because of that you have complete controll on how it behave.
 
```c#
public class MyNodeEditor : NodeEditor
{
    public override void StartEditor(NodeGraph graph)
    {
        base.StartEditor(graph);

        var ctx = new ContextMenuBuilder()
        .Add("greeter", () => Debug.Log("hello world!"))
        .Build();

        SetContextMenu(ctx);

        Events.OnGraphPointerClickEvent += OnGraphPointerClick;
    }

    private void OnGraphPointerClick(PointerEventData eventData)
    {
        DisplayContextMenu();
    }
}
```

This way, we can handle cases like without selection.

```c#
public class MyNodeEditor : NodeEditor
{
    ...
    
    private void OnGraphPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Right: DisplayContextMenu(); break;
            case PointerEventData.InputButton.Left:  CloseContextMenu();   break;
        }
    }

}
```





## Example node editor
There is a complete 

## How to create a node
## Make your custom node
## Anatomy of the graph
