using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor
{
    public class ResizablePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private RectTransform _parentTransform;
        private Vector2 _previousPointerPosition;
        private Vector2 _currentPointerPosition;

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentTransform,
                                                                    eventData.position,
                                                                    eventData.pressEventCamera,
                                                                    out _previousPointerPosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 sizeDelta = _parentTransform.sizeDelta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentTransform,
                                                        eventData.position,
                                                        eventData.pressEventCamera,
                                                        out _currentPointerPosition);

            Vector2 resizeValue = _currentPointerPosition - _previousPointerPosition;

            sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);

            _previousPointerPosition = _currentPointerPosition;
            _parentTransform.sizeDelta = sizeDelta;
            SetSize(sizeDelta);
        }

        public void SetSize(Vector2 size)
        {
            _parentTransform.sizeDelta = size;
        }

        public Vector2 GetSize()
        {
            return _parentTransform.sizeDelta;
        }

    }
}
