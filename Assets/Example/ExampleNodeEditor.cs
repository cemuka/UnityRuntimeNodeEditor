using RuntimeNodeEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExampleNodeEditor : NodeEditor
{
    private string _savePath;

    public override void StartEditor()
    {
        base.StartEditor();

        _savePath = Application.dataPath + "/NodeSystem/Resources/Graphs/graph.json";
        AddGraphContextMenuItem("nodes/float",          CreateFloatNode);
        AddGraphContextMenuItem("nodes/math op",       CreateMatOpNode);
        AddGraphContextMenuItem("graph/load",          ()=>LoadGraph(_savePath));
        AddGraphContextMenuItem("graph/save",          ()=>SaveGraph(_savePath));

    }
   
    protected override void OnNodePointerClick(Node node, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            AddNodeContextMenuItem("clear connections",    () => ClearConnections(node));
            AddNodeContextMenuItem("delete",               () => DeleteNode(node));

            DisplayNodeContexMenu(node);
        }
    }

    protected override void OnNodeConnectionPointerDown(string connId, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            AddConnectionContextMenuItem("delete connection", () => DisconnectConnection(connId));

            DisplayConnectionContexMenu(connId);
        }
    }


    //  context item actions
    private void CreateFloatNode()
    {
        var pos = Utility.GetLocalPointIn(graph.nodeContainer, Input.mousePosition);
        graph.Create("Prefabs/Nodes/FloatNode", pos);
        CloseContextMenu();
    }

    private void CreateGroup()
    {
        var pos = Utility.GetLocalPointIn(graph.nodeContainer, Input.mousePosition);
        graph.Create("Prefabs/Nodes/ResizeNode", pos);
        CloseContextMenu();
    }

    private void CreateMatOpNode()
    {
        var pos = Utility.GetLocalPointIn(graph.nodeContainer, Input.mousePosition);
        graph.Create("Prefabs/Nodes/MathOperationNode", pos);
        CloseContextMenu();
    }

    private void DeleteNode(Node node)
    {
        CloseContextMenu();
        graph.Delete(node);
    }

    private void DisconnectConnection(string line_id)
    {
        CloseContextMenu();
        graph.Disconnect(line_id);
    }

    private void ClearConnections(Node node)
    {
        CloseContextMenu();
        graph.ClearConnectionsOf(node);
    }

}