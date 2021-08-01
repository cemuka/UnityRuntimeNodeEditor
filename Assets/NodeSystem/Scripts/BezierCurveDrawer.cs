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

    private static UILineRenderer _lineRenderer;
    private static bool _hasRequest;
    private static Socket _draggingSocket;
    private static Dictionary<string, ConnectionDrawData> _connections;
    private static RectTransform _lineContainer;
    private static RectTransform _pointerLocator;

    public void Init()
    {
        _connections        = new Dictionary<string, ConnectionDrawData>();
        _lineContainer      = lineContainer;
        _pointerLocator     = pointerLocator;
        _lineRenderer       = CreateLine();
        _hasRequest         = false;
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
        _connections.Add(connId, new ConnectionDrawData(connId, from, target, CreateLine()));
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

    private static void DrawDragging(SocketHandle port)
    {
        Vector2 localPointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_lineContainer, Input.mousePosition, null, out localPointerPos);
        _pointerLocator.localPosition = localPointerPos;

        var pointList = new List<Vector2>();

        for (float i = 0; i < 120; i++)
        {
            var t = i / 120;
            pointList.Add(Utility.QuadraticCurve(GetLocalPoint(port.handle1.position),
                                                 GetLocalPoint(port.handle2.position),
                                                 GetLocalPoint(_pointerLocator.position),
                                                 t));
        }

        _lineRenderer.m_points = pointList.ToArray();
        _lineRenderer.SetVerticesDirty();
    }
   
    private static UILineRenderer CreateLine()
    {
        var lineGO                  = new GameObject("BezierLine");
        var linerenderer            = lineGO.AddComponent<UILineRenderer>();
        var lineRect                = lineGO.GetComponent<RectTransform>();
        
        lineGO.transform.SetParent(_lineContainer);

        lineRect.localPosition      = Vector3.zero;
        lineRect.localScale         = Vector3.one;
        lineRect.anchorMin          = Vector2.zero;
        lineRect.anchorMax          = Vector2.one;
        lineRect.Left(0);
        lineRect.Right(0);
        lineRect.Top(0);
        lineRect.Bottom(0);

        linerenderer.lineThickness  = 4f;
        linerenderer.color          = Color.yellow;
        linerenderer.raycastTarget  = false;

        return linerenderer;
    }

    private static Vector2 GetLocalPoint(Vector3 pos)
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

