using UnityEngine;

public class Utility 
{
    private static RectTransform _nodeContainer;
    private static RectTransform _contextMenuContainer;
    
    public Utility(RectTransform nodeContainer, RectTransform contextMenuContainer)
    {
        _nodeContainer          = nodeContainer;
        _contextMenuContainer   = contextMenuContainer;
    }


    public static Vector2 GetMousePosition()
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
}