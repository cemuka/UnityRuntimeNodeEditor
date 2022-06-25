using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace RuntimeNodeEditor
{
    public class BezierCurveDrawer : MonoBehaviour
    {
        public UILineRenderer RequestLine { get { return _lineRenderer; } }

        public RectTransform pointerLocator;
        public RectTransform lineContainer;
        [Header("Bezier settings")]
        public float vertexCount = 60f;
        public float lineThickness = 4f;  
        public Color connectionColor = Color.yellow;

        private RectTransform                _lineContainer;
        private RectTransform                _pointerLocator;
        private UILineRenderer               _lineRenderer;
        private bool                         _hasRequest;
        private Socket                       _draggingSocket;
        private IConnectionEvents            _events;
        private Dictionary<string, ConnectionDrawData> _connections;


        public void Init(IConnectionEvents events)
        {
            _connections    = new Dictionary<string, ConnectionDrawData>();
            _lineContainer  = lineContainer;
            _pointerLocator = pointerLocator;
            _lineRenderer   = CreateLine();
            _hasRequest     = false;
            _events         = events;

            CancelDrag();
        }

        public void UpdateDraw()
        {
            if (_connections.Count > 0)
            {
                foreach (var conn in _connections.Values)
                {
                    DrawConnection(conn.output, conn.input, conn.lineRenderer);
                }
            }

            if (_hasRequest)
            {
                DrawDragging(_draggingSocket.handle);
            }
        }

        public void Add(string connId, SocketHandle from, SocketHandle target)
        {
            var line = CreateLine();
            var trigger = line.gameObject.AddComponent<LinePointerListener>();
            trigger.Init(_events, connId);
            _connections.Add(connId, new ConnectionDrawData(connId, from, target, line));
        }

        public void Remove(string connId)
        {
            Destroy(_connections[connId].lineRenderer.gameObject);
            _connections.Remove(connId);
        }

        public void StartDrag(Socket from)
        {
            _draggingSocket = from;
            _hasRequest = true;
            _lineRenderer.gameObject.SetActive(_hasRequest);
        }

        public void CancelDrag()
        {
            _hasRequest = false;
            _lineRenderer.gameObject.SetActive(_hasRequest);
        }

        public void SetConnectionColor(string connId, Color color)
        {
            _connections[connId].lineRenderer.color = color;
        }
        //  drawing
        private void DrawConnection(SocketHandle port1, SocketHandle port2, UILineRenderer lineRenderer)
        {
            var pointList = new List<Vector2>();

            for (float i = 0; i < vertexCount; i++)
            {
                var t = i / vertexCount;
                pointList.Add(Utility.CubicCurve(GetLocalPoint(port1.handle1.position),
                                                 GetLocalPoint(port1.handle2.position),
                                                 GetLocalPoint(port2.handle1.position),
                                                 GetLocalPoint(port2.handle2.position),
                                                 t));
            }

            lineRenderer.m_points = pointList.ToArray();
            lineRenderer.SetVerticesDirty();
        }

        private void DrawDragging(SocketHandle port)
        {
	        Vector2 localPointerPos;
	        var mousePosition = Utility.GetMousePosition();
	        RectTransformUtility.ScreenPointToLocalPointInRectangle(_lineContainer, mousePosition, null, out localPointerPos);

            _pointerLocator.localPosition = localPointerPos;

            var pointList = new List<Vector2>();

            for (float i = 0; i < vertexCount; i++)
            {
                var t = i / vertexCount;
                pointList.Add(Utility.QuadraticCurve(GetLocalPoint(port.handle1.position),
                                                     GetLocalPoint(port.handle2.position),
                                                     GetLocalPoint(_pointerLocator.position),
                                                     t));
            }

            _lineRenderer.m_points = pointList.ToArray();
            _lineRenderer.SetVerticesDirty();
        }

        private UILineRenderer CreateLine()
        {
            return CreateLine(connectionColor);
        }

        private UILineRenderer CreateLine(Color color)
        {
            var lineGO          = new GameObject("BezierLine");
            var linerenderer    = lineGO.AddComponent<UILineRenderer>();
            var lineRect        = lineGO.GetComponent<RectTransform>();

            lineGO.transform.SetParent(_lineContainer);

            lineRect.localPosition = Vector3.zero;
            lineRect.localScale = Vector3.one;
            lineRect.Stretch();

            linerenderer.lineThickness  = lineThickness;
            linerenderer.color          = color;
            linerenderer.raycastTarget  = false;

            return linerenderer;
        }

        private Vector2 GetLocalPoint(Vector3 pos)
        {
            return Utility.GetLocalPointIn(_lineContainer, pos);
        }

        private class ConnectionDrawData
        {
            public readonly string id;
            public readonly SocketHandle output;
            public readonly SocketHandle input;
            public readonly UILineRenderer lineRenderer;

            public ConnectionDrawData(string id, SocketHandle port1, SocketHandle port2, UILineRenderer lineRenderer)
            {
                this.id = id;
                this.output = port1;
                this.input = port2;
                this.lineRenderer = lineRenderer;
            }
        }

    }

}