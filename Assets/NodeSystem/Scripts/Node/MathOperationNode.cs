using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class MathOperationNode : Node
{
    public TMP_Text resultText;
    public TMP_Dropdown dropdown;
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
        SetHeader("operation");
        SetValue(0);

        dropdown.AddOptions(new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData(MathOperations.Multiply.ToString()),
            new TMP_Dropdown.OptionData(MathOperations.Divide.ToString()),
            new TMP_Dropdown.OptionData(MathOperations.Add.ToString()),
            new TMP_Dropdown.OptionData(MathOperations.Substract.ToString())
        });

        dropdown.onValueChanged.AddListener(selected => 
        {
            OnConnectedValueUpdated();
        });
    }

    public override void OnConnection(IConnection conn)
    {
        _receivedConnections.Add(conn);
        conn.ValueUpdated += OnConnectedValueUpdated;

        OnConnectedValueUpdated();
    }

    private void OnConnectedValueUpdated()
    {
        List<float> inputValues = new List<float>();
        foreach (var c in _receivedConnections)
        {
            var type = c.GetType();

            switch (type)
            {
                case NodeType.Float:
                {
                    inputValues.Add(c.GetValue<float>());
                }
                break;
            }
        }


        float result = Calculate(inputValues);
        SetValue(result);
        ValueUpdatedEventInvoke();
        Display(result.ToString());
    }

    private void Display(string text)
    {
        resultText.text = text;
    }

    private float Calculate(List<float> values)
    {
        if (values.Count > 0)
        {
            var operation = (MathOperations)dropdown.value;
            switch (operation)
            {
                default :                           return values.Aggregate((x, y) => x * y);
                case MathOperations.Divide :        return values.Aggregate((x, y) => x / y);
                case MathOperations.Add :           return values.Aggregate((x, y) => x + y);
                case MathOperations.Substract :     return values.Aggregate((x, y) => x - y);
            }
        }
        else
        {
            return 0;
        }
    }
}