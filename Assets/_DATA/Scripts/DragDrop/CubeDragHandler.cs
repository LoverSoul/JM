using JM.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace JM.DragDrop
{
    /// <summary>
    /// Handles drag logic for cube UI element
    /// </summary>
    public class CubeDragHandler :
        MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        private const float DRAG_THRESHOLD = 10f;

        [Inject] private BottomBarView _bottomBar;

        private RectTransform _rect;
        private Canvas _canvas;

        private Vector2 _startAnchoredPos;
        private Vector2 _pointerOffset;     // offset between pointer and rect pivot
        private bool _dragging;
        private Transform _dragRoot;

        public System.Action<CubeView> OnBeginDragged;
        public System.Action<CubeView, Vector2> OnDropped;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            _canvas = GetComponentInParent<Canvas>();
        }

        public void Initialize(Transform dragRoot)
        {
            _dragRoot = dragRoot;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startAnchoredPos = _rect.anchoredPosition;
            _dragging = false;

            transform.SetParent(_dragRoot, true);

            // Calculate pointer offset relative to rect pivot
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_rect.parent,
                eventData.position,
                eventData.pressEventCamera,
                out var localPointerPos);

            _pointerOffset = _rect.anchoredPosition - localPointerPos;

            OnBeginDragged?.Invoke(GetComponent<CubeView>());
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragging &&
                Vector2.Distance(eventData.pressPosition, eventData.position) > DRAG_THRESHOLD)
            {
                _bottomBar.ScrollRect.enabled = false;
                _dragging = true;
            }

            if (!_dragging)
                return;

            // Convert screen position to local UI position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_rect.parent,
                eventData.position,
                eventData.pressEventCamera,
                out var localPointerPos);

            _rect.anchoredPosition = localPointerPos + _pointerOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _bottomBar.ScrollRect.enabled = true;

            OnDropped?.Invoke(GetComponent<CubeView>(), eventData.position);
        }

        public void ResetPosition()
        {
            _rect.anchoredPosition = _startAnchoredPos;
        }
    }
}
