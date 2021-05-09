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
    private Socket _requestSocket;
    private List<ConnectionData> _connections;

    public void Init()
    {
        _connections = new List<ConnectionData>();
        _lineRenderer = GenerateLine();
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
            DrawRequestConnection(_requestSocket);
        }
    }

    public void Add(Socket from, Socket to)
    {
        _connections.Add(new ConnectionData(from, to, GenerateLine()));
    }

    public void DrawRequest(Socket from)
    {
        _requestSocket = from;
        _hasRequest = true;
        _lineRenderer.gameObject.SetActive(_hasRequest);
    }

    public void CancelRequest()
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

    private void DrawConnection(Socket sock1, Socket sock2, UILineRenderer lineRenderer)
    {
        var pointList = new List<Vector2>();

        for (float i = 0; i < vertexCount; i++)
        {
            var t = i / vertexCount;
            pointList.Add(CubicCurve(sock1.handle1.position, sock1.handle2.position, sock2.handle1.position, sock2.handle2.position, t));
        }

        lineRenderer.m_points = pointList.ToArray();
        lineRenderer.SetVerticesDirty();
    }

    private void DrawRequestConnection(Socket socket)
    {
        Vector2 localPointerPos;
        var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(lineContainer, Input.mousePosition, null, out localPointerPos);
        PointerLocator.localPosition = localPointerPos;

        var pointList = new List<Vector2>();

        for (float i = 0; i < 120; i++)
        {
            var t = i / 120;
            pointList.Add(QuadraticCurve(socket.handle1.position, socket.handle2.position, PointerLocator.position, t));
        }

        _lineRenderer.m_points = pointList.ToArray();
        _lineRenderer.SetVerticesDirty();
    }
   
    private UILineRenderer GenerateLine()
    {
        var lineGO = new GameObject("BezierLine");
        lineGO.transform.SetParent(this.lineContainer);
        var linerenderer = lineGO.AddComponent<UILineRenderer>();

        linerenderer.material = new Material(Shader.Find("Sprites/Default"));
        linerenderer.lineThickness = 4f;
        linerenderer.material.color = Color.yellow;
        return linerenderer;
    }

}

public class ConnectionData
{
    public Socket output;
    public Socket input;
    public UILineRenderer lineRenderer;

    public ConnectionData(Socket conn1, Socket conn2, UILineRenderer lineRenderer)
    {
        this.output = conn1;
        this.input = conn2;
        this.lineRenderer = lineRenderer;
    }
}