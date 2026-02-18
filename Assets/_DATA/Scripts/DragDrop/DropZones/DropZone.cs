using JM.Views;
using UnityEngine;

namespace JM.DragDrop
{

    public class DropZone : MonoBehaviour
    {
        public RectTransform Rect => _rect;
        public RectTransform Canvas => _canvas;
        [SerializeField] protected RectTransform _canvas;
        [SerializeField] protected RectTransform _rect;
        [SerializeField] protected float _bottomOffset = 30f;

        private readonly Vector3[] _corners = new Vector3[4];

        public bool Contains(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rect, screenPoint);
        }
        
        public Vector3 GetBottomPosition(CubeView cube)
        {
            RectTransform canvas = cube.RectTransform.parent as RectTransform;

            Vector3 zonePivotWorld = _rect.position;
            Vector3 zonePivotLocal = canvas.InverseTransformPoint(zonePivotWorld);

            float targetY = zonePivotLocal.y + _bottomOffset;

            return new Vector3(
                cube.RectTransform.localPosition.x,
                targetY,
                cube.RectTransform.localPosition.z);
        }

    }
}