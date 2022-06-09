![node editor](./img/node_gif1.gif)

## Runtime Node Editor
Almost every node editor made in unity is using unity editor to make it.  
My goal was make it in runtime with unity ui.

- Prefab based workflow
- Context Menu for graph, nodes and connections
- Load and save a graph using node serializer
- Pan and zoom
- Legacy and new Input System 

![node editor](./img/node_gif2.gif)

## Example
Simply extend the `NodeEditor`.

```c#
public class ExampleNodeEditor : NodeEditor
{
    public override void StartEditor()
    {
        base.StartEditor();

        //  make your custom initialization here
    }
}
```

```c#
public class ApplicationStartup : MonoBehaviour
{
    public ExampleNodeEditor editor;    //  asigned in unity from hierarchy

    private void Start()
    {
        editor.StartEditor();
    }

    private void Update()
    {
        editor.UpdateEditor();
    }
}
```
Populate context menu by overriding methods below
```c#
protected override void OnGraphPointerClick(PointerEventData eventData)
{
}

protected override void OnNodePointerClick(Node node, PointerEventData eventData)
{
}

protected override void OnNodeConnectionPointerClick(string connId, PointerEventData eventData)
{
}
```
- graph context menu
```c#
protected override void OnGraphPointerClick(PointerEventData eventData)
{
    switch (eventData.button)
    {
        case PointerEventData.InputButton.Right: 
        {
            var ctx = new ContextMenuBuilder()
            .Add("nodes/float",          CreateFloatNode)
            .Add("nodes/math op",       CreateMatOpNode)
            .Add("graph/load",          ()=>LoadGraph(_savePath))
            .Add("graph/save",          ()=>SaveGraph(_savePath))
            .Build();

            SetContextMenu(ctx);
            DisplayContextMenu(); 
        }
        break;
        case PointerEventData.InputButton.Left: CloseContextMenu(); break;
    }
}
```
- node context menu
```c#
protected override void OnNodePointerClick(Node node, PointerEventData eventData)
{
    if (eventData.button == PointerEventData.InputButton.Right)
    {
            var ctx = new ContextMenuBuilder()
            .Add("duplicate",            () => DuplicateNode(node))
            .Add("clear connections",    () => ClearConnections(node))
            .Add("delete",               () => DeleteNode(node))
            .Build();

        SetContextMenu(ctx);
        DisplayContextMenu();
    }
}
```
- connection context menu
```c#
protected override void OnNodeConnectionPointerClick(string connId, PointerEventData eventData)
{
    if (eventData.button == PointerEventData.InputButton.Right)
    {
        var ctx = new ContextMenuBuilder()
        .Add("clear connection", () => DisconnectConnection(connId))
        .Build();

        SetContextMenu(ctx);
        DisplayContextMenu();
    }
}
```
Graph and nodes are all saved as prefab. Instead of handwriting graph and node visual elements, the simplest way I could find is making them a prefab and style as much as I can from the unity editor. Maintaining unityUI may get quickly overwhelming(layouts, nested rectTransforms, calculation of pointer position for nested objects, nested layout component issues, etc.)

That's been said, to create a new node:
```c#
public class MyAwesomeNode : Node
{
    public TMP_InputField valueField;   //  added from editor
    public SocketOutput outputSocket;   //  added from editor
    public SocketInput inputSocket;     //  added from editor

    public override void Setup()
    {
        Register(outputSocket);
        Register(inputSocket);

        SetHeader("float");
    }

    public override void OnSerialize(Serializer serializer)
    {
        //  save values on graph save
        serializer.Add("floatValue", valueField.text);

        //  it would be good idea to use JsonUtility for complex data
    }

    public override void OnDeserialize(Serializer serializer)
    {
        //  load values on graph load
        var value = serializer.Get("floatValue");
        valueField.SetTextWithoutNotify(value);
    }
}
```
To create a node from your editor, pass its path from `Resources` folder.
```c#
//  context item actions
private void CreateMyNode()
{
    graph.Create("Prefabs/Nodes/MyAwesomeNode");    //  your prefab path in resources
    CloseContextMenu();
}
```

Check out the complete expample in `ExampleScene` for more details.


This project is actively in development. 
Feel free to drop an issue for any suggestion or feedback.  


### LICENSE  
MIT  
Copyright (c) 2022 Cem Ugur Karacam
