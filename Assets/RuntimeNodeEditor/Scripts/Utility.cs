using UnityEngine;

namespace RuntimeNodeEditor
{
    public class Utility
    {
        public static Vector2 GetMousePosition()
        {
	        var mousePosition = Vector2.zero;
#if ENABLE_LEGACY_INPUT_MANAGER
	        mousePosition = Input.mousePosition;
#endif
#if ENABLE_INPUT_SYSTEM
	        mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif
            return mousePosition;
        }

        public static Vector2 GetLocalPointIn(RectTransform container, Vector3 pos, Camera eventCamera = null)
        {
            var point = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(container, pos, eventCamera, out point);
            return point;
        }

        public static Vector2 GetCtxMenuPointerPosition(RectTransform rect)
        {
	        Vector2 localPointerPos;
	        var success = false;
#if ENABLE_LEGACY_INPUT_MANAGER
            success = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
                                                                                  Input.mousePosition,
                                                                                  null,
                                                                                  out localPointerPos);
#endif
#if ENABLE_INPUT_SYSTEM
	        success = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
		        						UnityEngine.InputSystem.Mouse.current.position.ReadValue(),
		        						null,
		        						out localPointerPos);
#endif
            return localPointerPos;
        }



        public static T CreatePrefab<T>(string path, RectTransform parent)
        {
            var prefab = Resources.Load<GameObject>(path);
            var instance = GameObject.Instantiate(prefab, parent);
            var component = instance.GetComponent<T>();

            return component;
        }

        public static T CreateNodePrefab<T>(string path, RectTransform parent)
        {
            return CreatePrefab<T>(path, parent);
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
        public static RectTransform Left(this RectTransform rt, float x)
        {
            rt.offsetMin = new Vector2(x, rt.offsetMin.y);
            return rt;
        }

        public static RectTransform Right(this RectTransform rt, float x)
        {
            rt.offsetMax = new Vector2(-x, rt.offsetMax.y);
            return rt;
        }

        public static RectTransform Bottom(this RectTransform rt, float y)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, y);
            return rt;
        }

        public static RectTransform Top(this RectTransform rt, float y)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -y);
            return rt;
        }

        public static RectTransform Stretch(this RectTransform rt)
        {
            rt.anchorMin    = Vector2.zero;
            rt.anchorMax    = Vector2.one;
            rt.pivot        = Vector2.one * 0.5f;
            rt.Left(0f);
            rt.Right(0f);
            rt.Top(0f);
            rt.Bottom(0f);

            return rt;
        }

        public static bool IsRectTransformOverlap(this RectTransform rect1, RectTransform rect2)
        {
            float rect1MinX = rect1.position.x - rect1.rect.width / 2;
            float rect1MaxX = rect1.position.x + rect1.rect.width / 2;
            float rect1MinY = rect1.position.y - rect1.rect.height / 2;
            float rect1MaxY = rect1.position.y + rect1.rect.height / 2;
            float rect2MinX = rect2.position.x - rect2.rect.width / 2;
            float rect2MaxX = rect2.position.x + rect2.rect.width / 2;
            float rect2MinY = rect2.position.y - rect2.rect.height / 2;
            float rect2MaxY = rect2.position.y + rect2.rect.height / 2;

            bool notOverlap = rect1MaxX <= rect2MinX || rect2MaxX <= rect1MinX || rect1MaxY <= rect2MinY || rect2MaxY <= rect1MinY;

            return !notOverlap;
        }
    }
}