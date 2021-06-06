using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class BezierCurveDrawer : MonoBehaviour
{
    public RectTransform pointerLocator;
    public RectTransform lineContainer;
    [Header("Bezier settings")]
    public float vertexCount = 10;

    private UILineRenderer _lineRenderer;
    private bool _hasRequest;
    private Socket _draggingSocket;
    private Dictionary<int, ConnectionDrawData> _connections;

    public void Init()
    {
        _connections = new Dictionary<int, ConnectionDrawData>();
        _lineRenderer = CreateLine();
        _hasRequest = false;
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

    public void Add(int connId, SocketHandle from, SocketHandle target)
    {
        _connections.Add(connId, new ConnectionDrawData(connId, from, target, CreateLine()));
    }

    public void Remove(int connId)
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

    //  drawing
    private void DrawConnection(SocketHandle port1, SocketHandle port2, UILineRenderer lineRenderer)
    {
        var pointList = new List<Vector2>();

        for (float i = 0; i < vertexCount; i++)
        {
            var t = i / vertexCount;
            pointList.Add(Utility.CubicCurve(port1.handle1.position,
                                             port1.handle2.position,
                                             port2.handle1.position,
                                             port2.handle2.position,
                                             t));
        }

        lineRenderer.m_points = pointList.ToArray();
        lineRenderer.SetVerticesDirty();
    }

    private void DrawDragging(SocketHandle port)
    {
        Vector2 localPointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, Input.mousePosition, null, out localPointerPos);
        pointerLocator.localPosition = localPointerPos;

        var pointList = new List<Vector2>();

        for (float i = 0; i < 120; i++)
        {
            var t = i / 120;
            pointList.Add(Utility.QuadraticCurve(port.handle1.position,
                                                 port.handle2.position,
                                                 pointerLocator.position,
                                                 t));
        }

        _lineRenderer.m_points = pointList.ToArray();
        _lineRenderer.SetVerticesDirty();
    }
   
    private UILineRenderer CreateLine()
    {
        var lineGO = new GameObject("BezierLine");
        lineGO.transform.SetParent(this.lineContainer);
        var linerenderer = lineGO.AddComponent<UILineRenderer>();

        linerenderer.material = new Material(Shader.Find("Sprites/Default"));
        linerenderer.lineThickness = 4f;
        linerenderer.material.color = Color.yellow;
        return linerenderer;
    }

    private class ConnectionDrawData
    {
        public int id;
        public SocketHandle output;
        public SocketHandle input;
        public UILineRenderer lineRenderer;

        public ConnectionDrawData(int id, SocketHandle port1, SocketHandle port2, UILineRenderer lineRenderer)
        {
            this.id = id;
            this.output = port1;
            this.input = port2;
            this.lineRenderer = lineRenderer;
        }
    }
}

