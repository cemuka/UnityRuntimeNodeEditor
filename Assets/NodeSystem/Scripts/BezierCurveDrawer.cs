using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class BezierCurveDrawer : MonoBehaviour
{
    public RectTransform PointerLocator;
    public RectTransform lineContainer;
    public RectTransform nodeContainer;
    [Header("Bezier settings")]
    public float vertexCount = 10;

    private UILineRenderer _lineRenderer;
    private bool _hasRequest;
    private Socket _draggingSocket;
    private List<ConnectionDrawData> _connections;

    public void Init()
    {
        _connections = new List<ConnectionDrawData>();
        _lineRenderer = CreateLine();
        _hasRequest = false;
    }
    
    public void UpdateDraw()
    {
        if (_connections.Count > 0)
        {
            _connections.ForEach( conn => DrawConnection(conn.output, conn.input, conn.lineRenderer));
        }

        if (_hasRequest)
        {
            DrawDragging(_draggingSocket.handle);
        }
    }

    public void Add(Socket from, Socket target)
    {
        _connections.Add(new ConnectionDrawData(from.handle, target.handle, CreateLine()));
    }

    public void Remove(){}

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
    private Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        var p0 = Vector3.Lerp(a, b, t);
        var p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }
    
    private Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        var p0 = QuadraticCurve(a, b, c, t);
        var p1 = QuadraticCurve(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }

    private void DrawConnection(SocketHandle port1, SocketHandle port2, UILineRenderer lineRenderer)
    {
        var pointList = new List<Vector2>();

        for (float i = 0; i < vertexCount; i++)
        {
            var t = i / vertexCount;
            pointList.Add(CubicCurve(port1.handle1.position, port1.handle2.position, port2.handle1.position, port2.handle2.position, t));
        }

        lineRenderer.m_points = pointList.ToArray();
        lineRenderer.SetVerticesDirty();
    }

    private void DrawDragging(SocketHandle port)
    {
        Vector2 localPointerPos;
        var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, Input.mousePosition, null, out localPointerPos);
        PointerLocator.localPosition = localPointerPos;

        var pointList = new List<Vector2>();

        for (float i = 0; i < 120; i++)
        {
            var t = i / 120;
            pointList.Add(QuadraticCurve(port.handle1.position, port.handle2.position, PointerLocator.position, t));
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
        public SocketHandle output;
        public SocketHandle input;
        public UILineRenderer lineRenderer;

        public ConnectionDrawData(SocketHandle port1, SocketHandle port2, UILineRenderer lineRenderer)
        {
            this.output = port1;
            this.input = port2;
            this.lineRenderer = lineRenderer;
        }
    }
}

