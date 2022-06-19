
Welcome to the RuntimeNodeEditor documentation. 

# Quick overview

- RuntimeNodeEditor is a unityUI based node editor and works in playmode.

- A connection happens between a `InputSocket.cs` and `OutputSocket.cs` all derived from `Socket.cs`.

- A `Node.cs` is a container for sockets. It listens connection events for its sockets.

- A `NodeGraph.cs` can create nodes and draws connections between sockets. Can be serialized into json file.

- `NodeEditor.cs` is a MonoBehavior class and lives in the scene. Can listen graph events. It has a simple context menu feature.

- It is possible to have multiple node editors in a scene. Each editor will 

- Works completely event based. Integrates with unity `EventSystem` to listen pointer events for sockets, nodes and graph.

- Customizable for every detail. Some parts of the framework require manual work 


# Getting started

RuntimeNodeEditor is intended as a framework to work easily in a unity scene. 
It can be setup with a minimal configuration and its event based flow allows users to create their system on top of it. 

## Quick start





## Core concepts

- Socket and Connection
- Node
- NodeGraph
- NodeEditor
- ContextMenu
- Serializing

### Socket and Connection

> `output(value)` --> connection --> `input`

A single connection happens between an input and an output socket. 
This is a one-direction communication from the output to input. An input may accept single or multiple connections. 

`SocketInput` and `SocketOutput` is implemented to work as generic sockets derived from abstract `Socket`. 

`SocketOutput` implements `IOutput` interface to make it easy to share and read the incoming value as an object.

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
    public SocketOutput myOutput;
}
```

`Node` is a hub for sockets. It registers and controlls its sockets when a connection invoked. 


When a connection established, input socket's owner node invokes the `OnConnectionEvent` event.


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

This way it is possible to listen connection and disconnection events to get information from outputs

### NodeGraph

`NodeGraph` will contain nodes, connections and connection curves. Graph can create a node by providing its prefab path from `Resources` folder.

```c#
public class MyNodeEditor : NodeEditor
{
    ...

    private void SomeMethod()
    {
        Graph.Create("path/to/prefab/in/Resources");
    }
}

```

Following a prefab path is a design decision. It has a certain benefits to make prefab instantiation easy (and serialization)but comes with some manual work for sure. 

Graph connects sockets by listening pointer events by default. It is possible to override its virtual methods or make costumizations.

It also listen the pointer events from graph, nodes and connections. Check out `SignalSystem.cs` for the complete events.


### NodeEditor

`NodeEditor is the central part of the framework. It can be simplified a complete editor like below.

```c#
public class MyNodeEditor : NodeEditor
{
    public override void StartEditor(NodeGraph graph)
    {
        base.StartEditor(graph);


        //  Create, delete or duplicate nodes
        
        //  Show or hide context menu
        
        //  Set context menu actions

        //  Listen events
    }
}

```

### Context menu

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

### Serializing

A graph can be serialized and deserialized. 

Note that, in order to use `Duplicate` method in NodeGrap target node must implement serialization. Here is a sample node from `Examples` folder.

```c#

//  FloatNode.cs
public class FloatNode : Node
{
    public TMP_InputField valueField;
    
    ...

    public override void OnSerialize(Serializer serializer)
    {
        serializer.Add("floatValue", valueField.text);
    }

    public override void OnDeserialize(Serializer serializer)
    {
        var value = serializer.Get("floatValue");
        valueField.SetTextWithoutNotify(value);

        ...
    }

    ...
}

```

Serializitaion produces a json string and NodeGraph can write it as a file to specified path. It uses unity `JsonUtility`.

Since its text based, `JsonUtility` can be used further like below

```c#

//  Serialize
var jsonData = JsonUtility.ToJson(myCustomData);
serializer.Add("myData", jsonData);


//  Deserialize
var jsonData = serializer.Get("myData");   
myCustomData = JsonUtility.FromJson<CustomJsonData>(jsonData);    
```

Finally save to a file or load from a file.

```c#

//  ExampleNodeEditor.cs
public class MyNodeEditor : NodeEditor
{
    ...

    private void SaveGraph(string savePath)
    {
        CloseContextMenu();
        Graph.Save(savePath);
    }

    private void LoadGraph(string savePath)
    {
        CloseContextMenu();
        Graph.Clear();
        Graph.Load(savePath);
    }

    ...
}

```


## How to create a node
## Make your custom node
## Anatomy of the graph

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





## Example editors in project
There is a complete 
