using UnityEngine;

namespace RuntimeNodeEditor
{
    public class Utility
    {
        private static RectTransform _nodeContainer;
        private static RectTransform _contextMenuContainer;

        public static void Initialize(RectTransform nodeContainer, RectTransform contextMenuContainer)
        {
            _nodeContainer = nodeContainer;
            _contextMenuContainer = contextMenuContainer;
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
	        var success = false;
	        #if ENABLE_LEGACY_INPUT_MANAGER
            success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_contextMenuContainer,
                                                                                  Input.mousePosition,
                                                                                  null,
                                                                                  out localPointerPos);
	        #endif
	        #if ENABLE_INPUT_SYSTEM
	        success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_contextMenuContainer,
		        						UnityEngine.InputSystem.Mouse.current.position.ReadValue(),
		        						null,
		        						out localPointerPos);
	        #endif
            return localPointerPos;
        }



        public static T CreatePrefab<T>(string path, Transform parent)
        {
            var prefab = Resources.Load<GameObject>(path);
            var instance = GameObject.Instantiate(prefab, parent);
            var component = instance.GetComponent<T>();

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


        public static Transform FindTopmostCanvas(Transform currentObject)
        {
            var canvases = currentObject.GetComponentsInParent<Canvas>();
            if (canvases.Length == 0)
            {
                return null;
            }

            return canvases[canvases.Length - 1].transform;
        }

        public static void UpdateLayout(UnityEngine.UI.LayoutGroup layout)
        {
            if (layout == null)
            {
                return;
            }

            layout.CalculateLayoutInputHorizontal();
            layout.SetLayoutHorizontal();
            layout.CalculateLayoutInputVertical();
            layout.SetLayoutVertical();
        }


        public static T GetOrAddComponent<T>(Component obj)
            where T : Component
        {
            var component = obj.GetComponent<T>();
            if (component == null)
            {
                component = obj.gameObject.AddComponent<T>();
            }

            return component;
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