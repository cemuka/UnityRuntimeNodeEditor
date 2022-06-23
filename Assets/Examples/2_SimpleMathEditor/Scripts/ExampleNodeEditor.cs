using System;
using RuntimeNodeEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor.Examples
{
    public class ExampleNodeEditor : NodeEditor
    {
        private string _savePath;

        public override void StartEditor(NodeGraph graph)
        {
            base.StartEditor(graph);

            _savePath = Application.dataPath + "/Examples/2_SimpleMathEditor/Resources/graph.json";
            
            Events.OnGraphPointerClickEvent           += OnGraphPointerClick;
            Events.OnNodePointerClickEvent            += OnNodePointerClick;
            Events.OnConnectionPointerClickEvent      += OnNodeConnectionPointerClick;
            Events.OnSocketConnect                    += OnConnect;

            Graph.SetSize(Vector2.one * 20000);
        }

        private void OnConnect(SocketInput arg1, SocketOutput arg2)
        {
            Graph.drawer.SetConnectionColor(arg2.connection.connId, Color.green);
        }

        private void OnGraphPointerClick(PointerEventData eventData)
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

        private void SaveGraph(string savePath)
        {
            CloseContextMenu();
            Graph.SaveFile(savePath);
        }

        private void LoadGraph(string savePath)
        {
            CloseContextMenu();
            Graph.Clear();
            Graph.LoadFile(savePath);
        }

        private void OnNodePointerClick(Node node, PointerEventData eventData)
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

        private void OnNodeConnectionPointerClick(string connId, PointerEventData eventData)
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
            Graph.Create("Nodes/FloatNode");
            CloseContextMenu();
        }

        private void CreateMatOpNode()
        {
            Graph.Create("Nodes/MathOperationNode");
            CloseContextMenu();
        }

        private void DeleteNode(Node node)
        {
            Graph.Delete(node);
            CloseContextMenu();
        }
        
        private void DuplicateNode(Node node)
        {
            Graph.Duplicate(node);
            CloseContextMenu();
        }

        private void DisconnectConnection(string line_id)
        {
            Graph.Disconnect(line_id);
            CloseContextMenu();
        }

        private void ClearConnections(Node node)
        {
            Graph.ClearConnectionsOf(node);
            CloseContextMenu();
        }

    }
}