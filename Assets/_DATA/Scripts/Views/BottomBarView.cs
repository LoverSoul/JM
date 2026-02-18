using UnityEngine;
using UnityEngine.UI;

namespace JM.Views
{
    public class BottomBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _content;

        public ScrollRect ScrollRect => _scrollRect;
        public RectTransform Content => _content;
        public RectTransform Canvas => _canvas;
    }
}