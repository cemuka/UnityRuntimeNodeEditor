using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeNodeEditor.Examples
{
    public class MathOperationNode : Node
    {
        public TMP_Text resultText;
        public TMP_Dropdown dropdown;
        public SocketInput inputSocket;
        public SocketOutput outputSocket;

        private List<IOutput> _incomingOutputs;


        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();

            Register(outputSocket);
            Register(inputSocket);

            SetHeader("operation");
            outputSocket.SetValue(0f);

            dropdown.AddOptions(new List<TMP_Dropdown.OptionData>()
            {
                new TMP_Dropdown.OptionData(MathOperations.Multiply.ToString()),
                new TMP_Dropdown.OptionData(MathOperations.Divide.ToString()),
                new TMP_Dropdown.OptionData(MathOperations.Add.ToString()),
                new TMP_Dropdown.OptionData(MathOperations.Subtract.ToString())
            });

            dropdown.onValueChanged.AddListener(selected =>
            {
                OnConnectedValueUpdated();
            });

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;
        }

        public void OnConnection(SocketInput input, IOutput output)
        {
            output.ValueUpdated += OnConnectedValueUpdated;
            _incomingOutputs.Add(output);

            OnConnectedValueUpdated();
        }

        public void OnDisconnect(SocketInput input, IOutput output)
        {
            output.ValueUpdated -= OnConnectedValueUpdated;
            _incomingOutputs.Remove(output);

            OnConnectedValueUpdated();
        }

        public override void OnSerialize(Serializer serializer)
        {
            var output = outputSocket.GetValue<float>();
            serializer.Add("outputValue", output.ToString())
                        .Add("opType", dropdown.value.ToString());
        }

        public override void OnDeserialize(Serializer serializer)
        {
            var opType = int.Parse(serializer.Get("opType"));
            dropdown.SetValueWithoutNotify(opType);

            var outputValue = serializer.Get("outputValue");
            Display(outputValue);
        }

        private void OnConnectedValueUpdated()
        {
            List<float> incomingValues = new List<float>();
            foreach (var c in _incomingOutputs)
            {
                incomingValues.Add(c.GetValue<float>());
            }

            float result = Calculate(incomingValues);
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
                    default: return values.Aggregate((x, y) => x * y);
                    case MathOperations.Divide: return values.Aggregate((x, y) => x / y);
                    case MathOperations.Add: return values.Aggregate((x, y) => x + y);
                    case MathOperations.Subtract: return values.Aggregate((x, y) => x - y);
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
