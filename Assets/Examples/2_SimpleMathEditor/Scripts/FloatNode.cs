using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace RuntimeNodeEditor.Examples
{
    public class FloatNode : Node
    {
        public TMP_InputField valueField;
        public SocketOutput outputSocket;

        public override void Setup()
        {
            Register(outputSocket);
            SetHeader("float");

            valueField.text = "0";
            HandleFieldValue(valueField.text);

            valueField.contentType = TMP_InputField.ContentType.DecimalNumber;
            valueField.onEndEdit.AddListener(HandleFieldValue);
        }

        private void HandleFieldValue(string value)
        {
            float floatValue = float.Parse(value);
            outputSocket.SetValue(floatValue);
        }

        public override void OnSerialize(Serializer serializer)
        {
            serializer.Add("floatValue", valueField.text);
        }

        public override void OnDeserialize(Serializer serializer)
        {
            var value = serializer.Get("floatValue");
            valueField.SetTextWithoutNotify(value);

            HandleFieldValue(value);
        }
    }
}