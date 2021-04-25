using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FloatNode : Node
{
    public TMP_InputField valueField;
    public Socket outputSocket;

    public override void Init(Vector2 pos)
    {
        base.Init(pos);
        outputSocket.Init(this);
        SetType(NodeType.Float);
        SetHeader("float");
        
        valueField.text = "0";
        HandleInputValue(valueField.text);

        valueField.contentType = TMP_InputField.ContentType.DecimalNumber;
        valueField.onValueChanged.AddListener(HandleInputValue);
    }

    private void HandleInputValue(string value)
    {
        float floatValue = float.Parse(value);
        SetValue(floatValue);

        ValueUpdatedEventInvoke();
    }
}