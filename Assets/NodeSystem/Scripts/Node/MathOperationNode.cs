using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class MathOperationNode : Node
{
    public TMP_Text         resultText;
    public TMP_Dropdown     dropdown;
    public SocketInput      inputSocket;
    public SocketOutput     outputSocket;

    private List<IOutput> _receivedOutputs;

    public override void Init(Vector2 pos)
    {
        base.Init(pos);
        _receivedOutputs = new List<IOutput>();
        inputSocket.Init(this);
        outputSocket.Init(this);
        SetType(NodeType.Float);
        SetHeader("operation");
        outputSocket.SetValue(0f);

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

    public override void OnConnection(SocketInput port, IOutput output)
    {
        _receivedOutputs.Add(output);
        output.ValueUpdated += OnConnectedValueUpdated;

        OnConnectedValueUpdated();
    }

    private void OnConnectedValueUpdated()
    {
        List<float> inputValues = new List<float>();
        foreach (var c in _receivedOutputs)
        {
            inputValues.Add(c.GetValue<float>());
        }

        float result = Calculate(inputValues);
        outputSocket.SetValue(result);
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