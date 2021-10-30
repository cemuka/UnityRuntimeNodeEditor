using RuntimeNodeEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExampleNodeEditor : NodeEditor
{
    private string _savePath;

    public override void StartEditor()
    {
        base.StartEditor();

        _savePath = Application.dataPath + "/Resources/graph.json";

    }

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
   
    protected override void OnNodePointerClick(Node node, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var ctx = new ContextMenuBuilder()
	            .Add("duplicate",    () => DuplicateNode(node))
            .Add("clear connections",    () => ClearConnections(node))
            .Add("delete",               () => DeleteNode(node))
            .Build();

            SetContextMenu(ctx);
            DisplayContextMenu();
        }
    }

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


    //  context item actions
    private void CreateFloatNode()
    {
        graph.Create("Prefabs/Nodes/FloatNode");
        CloseContextMenu();
    }

    private void CreateMatOpNode()
    {
        graph.Create("Prefabs/Nodes/MathOperationNode");
        CloseContextMenu();
    }

    private void DeleteNode(Node node)
    {
        graph.Delete(node);
        CloseContextMenu();
    }
    
	private void DuplicateNode(Node node)
	{
		graph.Duplicate(node);
		CloseContextMenu();
	}

    private void DisconnectConnection(string line_id)
    {
        graph.Disconnect(line_id);
        CloseContextMenu();
    }

    private void ClearConnections(Node node)
    {
        graph.ClearConnectionsOf(node);
        CloseContextMenu();
    }

}