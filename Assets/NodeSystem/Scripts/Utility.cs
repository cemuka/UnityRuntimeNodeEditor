using UnityEngine;

public class Utility 
{
    private static RectTransform _nodeContainer;
    private static RectTransform _contextMenuContainer;
    
    public static void Initialize(RectTransform nodeContainer, RectTransform contextMenuContainer)
    {
        _nodeContainer          = nodeContainer;
        _contextMenuContainer   = contextMenuContainer;
    }


    public static Vector2 GetLocalPointIn(RectTransform container, Vector3 pos, Camera eventCamera = null)
    {
        var point = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(container, pos, eventCamera, out point);
        return point;
    }

    public static Vector2 GetCtxMenuPointerPosition()
    {
        Vector2 localPointerPos;
        var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_contextMenuContainer,
                                                                              Input.mousePosition,
                                                                              null,
                                                                              out localPointerPos);
    
        return localPointerPos;
    }

    public static T CreatePrefab<T>(string path, Transform parent)
    {
        var prefab          = Resources.Load<GameObject>(path);
        var instance        = GameObject.Instantiate(prefab, parent);
        var component       = instance.GetComponent<T>();

        return component;
    }

    public static T CreateNodePrefab<T>(string path)
    {
        return CreatePrefab<T>(path, _nodeContainer);
    }

    public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        var p0 = Vector3.Lerp(a, b, t);
        var p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        var p0 = QuadraticCurve(a, b, c, t);
        var p1 = QuadraticCurve(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }
}

public static class RectTransformExtensions
{
    public static RectTransform Left( this RectTransform rt, float x )
    {
        rt.offsetMin = new Vector2( x, rt.offsetMin.y );
        return rt;
    }

    public static RectTransform Right( this RectTransform rt, float x )
    {
        rt.offsetMax = new Vector2( -x, rt.offsetMax.y );
        return rt;
    }

    public static RectTransform Bottom( this RectTransform rt, float y )
    {
        rt.offsetMin = new Vector2( rt.offsetMin.x, y );
        return rt;
    }

    public static RectTransform Top( this RectTransform rt, float y )
    {
        rt.offsetMax = new Vector2( rt.offsetMax.x, -y );
        return rt;
    }
}
