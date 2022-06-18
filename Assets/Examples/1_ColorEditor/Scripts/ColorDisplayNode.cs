using UnityEngine;
using UnityEngine.UI;

namespace RuntimeNodeEditor.Examples
{
    public class ColorDisplayNode : Node
    {
        public Image display;

        public SocketInput rInput;
        public SocketInput gInput;
        public SocketInput bInput;

        private IOutput _rChannel;
        private IOutput _gChannel;
        private IOutput _bChannel;

        public override void Setup()
        {
            rInput.connectionType = ConnectionType.Single;
            gInput.connectionType = ConnectionType.Single;
            bInput.connectionType = ConnectionType.Single;

            Register(rInput);
            Register(gInput);
            Register(bInput);

            SetHeader("display");

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;
        }

        public void OnConnection(SocketInput input, IOutput output)
        {
            if (input == rInput) { _rChannel = output; _rChannel.ValueUpdated += RefreshDisplay; }
            if (input == gInput) { _gChannel = output; _gChannel.ValueUpdated += RefreshDisplay; }
            if (input == bInput) { _bChannel = output; _bChannel.ValueUpdated += RefreshDisplay; }

            RefreshDisplay();
        }

        public void OnDisconnect(SocketInput input, IOutput output)
        {
            if (input == rInput) { _rChannel.ValueUpdated -= RefreshDisplay; _rChannel = null;   }
            if (input == gInput) { _gChannel.ValueUpdated -= RefreshDisplay; _gChannel = null;   }
            if (input == bInput) { _bChannel.ValueUpdated -= RefreshDisplay; _bChannel = null;   }

            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            float red = 0f;
            float green = 0f;
            float blue = 0f;

            if (_rChannel != null) { red    = _rChannel.GetValue<float>(); }
            if (_gChannel != null) { green  = _gChannel.GetValue<float>(); }
            if (_bChannel != null) { blue   = _bChannel.GetValue<float>(); }

            display.color = new Color(red, green, blue);
            Debug.Log(display.color);
        }
    }
}
