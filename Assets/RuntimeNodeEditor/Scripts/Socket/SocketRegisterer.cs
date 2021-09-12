using System;
using System.Collections.Generic;

namespace RuntimeNodeEditor
{
    public class SocketRegisterer
    {
        public List<SocketOutput> outputs;
        public List<SocketInput> inputs;

        public SocketRegisterer()
        {
            outputs = new List<SocketOutput>();
            inputs = new List<SocketInput>();
        }

        public void Register(SocketOutput output)
        {
            outputs.Add(output);
        }

        public void Register(SocketInput input)
        {
            inputs.Add(input);
        }
    }
}