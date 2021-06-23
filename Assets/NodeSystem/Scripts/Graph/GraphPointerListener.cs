using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphPointerListener : MonoBehaviour, IPointerClickHandler, IDragHandler
{

    private RectTransform _rectTransform;
    private Vector2 _zoomCenterPos;
    private float    _currentZoom;
    private float    _mouseWheelSensitivity;
    private bool    _isDragging = false;
    private float    _minZoom;
    private float    _maxZoom;

    private List<RaycastResult> _raycastResults;
    private PointerEventData    _currentPointerData;
    
    public static event Action<PointerEventData> GraphPointerClickEvent;
    public static event Action<PointerEventData> GraphPointerDragEvent;

    public void Init(RectTransform background, float minZoom, float maxZoom)
    {
        _rectTransform          = background;
        _minZoom                = minZoom;
        _maxZoom                = maxZoom;
        _currentZoom            = 1;
        _mouseWheelSensitivity  = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GraphPointerClickEvent?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        GraphPointerDragEvent?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }

//     private void ConstraitPosition()
//     {
//         RectTransform rectTransform = (RectTransform)transform;

//         RectTransform parentTransform = (RectTransform)rectTransform.parent;
//         Vector2 viewportSize = new Vector2(parentTransform.rect.width, parentTransform.rect.height);
//         Vector2 contentSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

//         Vector2 dragMax = new Vector2(0, rectTransform.sizeDelta.y - viewportSize.y);
//         Vector2 dragMin = new Vector2(-(rectTransform.sizeDelta.x - viewportSize.x), 0);

//         rectTransform.anchoredPosition = new Vector2(
//             Mathf.Min(Mathf.Max(dragMin.x, rectTransform.anchoredPosition.x), dragMax.x),
//             Mathf.Min(Mathf.Max(dragMin.y, rectTransform.anchoredPosition.y), dragMax.y)
//         );
//     }

    public void OnUpdate()
    {
        //pc input
        // float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        float scrollWheelInput = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollWheelInput) > float.Epsilon)
        {
            _currentZoom *= 1 + scrollWheelInput * _mouseWheelSensitivity;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            _zoomCenterPos = (Vector2)Input.mousePosition;

            Vector2 beforePointInContent;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, _zoomCenterPos, null, out beforePointInContent);

            Vector2 pivotPosition = new Vector3(_rectTransform.pivot.x * _rectTransform.rect.size.x, _rectTransform.pivot.y * _rectTransform.rect.size.y);
            Vector2 posFromBottomLeft = pivotPosition + beforePointInContent;
            SetPivot(_rectTransform, new Vector2(posFromBottomLeft.x / _rectTransform.rect.width, posFromBottomLeft.y / _rectTransform.rect.height));

            if (Mathf.Abs(_rectTransform.localScale.x - _currentZoom) > 0.001f)
            {
                _rectTransform.localScale =  Vector3.one * _currentZoom; 
            }
        }
    }

    public static void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y) * rectTransform.localScale.x;
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }

}
