using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
	public class GraphPointerListener : MonoBehaviour, IPointerClickHandler, IDragHandler, IScrollHandler
    {

        private RectTransform   _rectTransform;
        private Vector2         _zoomCenterPos;
        private float           _currentZoom;
        private float           _mouseWheelSensitivity;
        private float           _minZoom;
        private float           _maxZoom;
        private SignalSystem    _signalSystem;

        public void Init(SignalSystem signalSystem, RectTransform background, float minZoom, float maxZoom)
        {
            _signalSystem = signalSystem;
            _rectTransform = background;
            _minZoom = minZoom;
            _maxZoom = maxZoom;
            _currentZoom = 1;
            _mouseWheelSensitivity = 1;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _signalSystem.InvokeGraphPointerClick(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _signalSystem.InvokeGraphPointerDrag(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (Mathf.Abs(eventData.scrollDelta.y) > float.Epsilon)
            {
                _currentZoom *= 1 + eventData.scrollDelta.y * _mouseWheelSensitivity;
	            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
	            _zoomCenterPos = Utility.GetMousePosition();

                Vector2 beforePointInContent;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, _zoomCenterPos, null, out beforePointInContent);

                Vector2 pivotPosition = new Vector3(_rectTransform.pivot.x * _rectTransform.rect.size.x, _rectTransform.pivot.y * _rectTransform.rect.size.y);
                Vector2 posFromBottomLeft = pivotPosition + beforePointInContent;
                SetPivot(_rectTransform, new Vector2(posFromBottomLeft.x / _rectTransform.rect.width, posFromBottomLeft.y / _rectTransform.rect.height));

                if (Mathf.Abs(_rectTransform.localScale.x - _currentZoom) > 0.001f)
                {
                    _rectTransform.localScale = Vector3.one * _currentZoom;
                }
            }

            _signalSystem.InvokeGraphPointerScroll(eventData);
        }

        public void SetPivot(RectTransform rectTransform, Vector2 pivot)
        {
            Vector2 size = rectTransform.rect.size;
            Vector2 deltaPivot = rectTransform.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y) * rectTransform.localScale.x;
            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }

    }
}