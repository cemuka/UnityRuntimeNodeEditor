using System;
using UnityEngine;

namespace RuntimeNodeEditor
{
    [Serializable]
    public class NodeData
    {
        public string id;
        public SerializedValue[] values;
        public float posX;
        public float posY;
        public string path;
        public string[] inputSocketIds;
        public string[] outputSocketIds;
    }

    [Serializable]
    public class ConnectionData
    {
        public string id;
        public string outputSocketId;
        public string inputSocketId;
    }

    [System.Serializable]
    public class SerializedValue
    {
        public string key;
        public string value;
    }

    public class GraphData
    {
        public NodeData[] nodes;
        public ConnectionData[] connections;
    }
}