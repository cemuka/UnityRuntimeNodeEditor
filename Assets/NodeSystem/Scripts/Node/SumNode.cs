using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class SumNode : Node
{
    public TMP_Text resultText;
    public Socket inputSocket;
    public Socket outputSocket;

    private List<IConnection> _receivedConnections;

    public override void Init(Vector2 pos)
    {
        base.Init(pos);
        _receivedConnections = new List<IConnection>();
        inputSocket.Init(this);
        outputSocket.Init(this);
        SetType(NodeType.Float);
    }

    public override void OnConnection(IConnection conn)
    {
        _receivedConnections.Add(conn);
        conn.ValueUpdated += OnConnectedValueUpdated;

        OnConnectedValueUpdated();
    }

    private void OnConnectedValueUpdated()
    {
        float result = 0;
        foreach (var c in _receivedConnections)
        {
            var type = c.GetType();

            switch (type)
            {
                case NodeType.Float:
                {
                    result += c.GetValue<float>();
                }
                break;
            }
        }

        SetValue(result);
        ValueUpdatedEventInvoke();
        Display(result.ToString());
    }

    private void Display(string text)
    {
        resultText.text = text;
    }
}