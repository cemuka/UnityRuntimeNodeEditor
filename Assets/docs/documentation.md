
Welcome to the RuntimeNodeEditor documentation. 

# Quick overview

- RuntimeNodeEditor is a unityUI based node editor and works in playmode.

- A connection happens between a `InputSocket.cs` and `OutputSocket.cs` all derived from `Socket.cs`.

- A `Node.cs` is a container for sockets. It listens connection events for its sockets.

- A `NodeGraph.cs` can create nodes and draws connections between sockets. Can be serialized into json file.

- `NodeEditor.cs` is a MonoBehavior class and lives in the scene. Can listen graph events. It has a simple context menu feature.

- It is possible to have multiple node editors in a scene. Each editor will 

- Works completely event based. Integrates with unity `EventSystem` to listen pointer events for sockets, nodes and graph.


# Getting started

RuntimeNodeEditor is intended as a framework to work easily in a unity scene. 
It can be setup with a minimal configuration and its event based flow allows users to create their system on top of it. 

## Core concepts

- Socket and Connection
- Node
- NodeGraph
- NodeEditor
- ContextMenu

### Socket and Connection

> `output(value)` --> connection --> `input`

A single connection happens between an input and an output socket. 
This is a one-direction communication from the output to input. An input may accept single or multiple connections. 

`InputSocket` and `OutputSocket` is implemented to work as generic sockets derived from abstract `Socket`. 

`OutputSocket` implements `IOutput` interface to make it easy to share and read the incoming value as an object.

```c#
public interface IOutput
{
    T GetValue<T>();
    event Action ValueUpdated;
}
```



### Node

```c#
public class SampleNode : Node
{
    public OutputSocket myOutput;
}
```

A node is a hub for sockets. It registers and controlls its sockets when a connection invoked. 


When a connection succeeded, input socket's owner node invokes the `OnConnectionEvent` event.


```c#
public class SampleNode : Node
{
    ...
    
    //  fields
    //  ui objects fields
    //  input or output sockets 

    ...

    public override void Setup()
    {
        //  setup your custom node here

        OnConnectionEvent += OnConnection;
        OnDisconnectEvent += OnDisconnect;
    }

    public void OnConnection(SocketInput input, IOutput output)
    {
        output.ValueUpdated += OnConnectedValueUpdated;
    }

    public void OnDisconnect(SocketInput input, IOutput output)
    {
        output.ValueUpdated -= OnConnectedValueUpdated;
    }
}
```


### NodeGraph
### NodeEditor
### ContextMenu





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
