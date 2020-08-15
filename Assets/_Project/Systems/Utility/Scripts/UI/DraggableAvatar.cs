using UnityEngine;

namespace SpaceGame.Utility.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableAvatar : MonoBehaviour
    {
        [SerializeField] private float _alphaWhileDragged = 0.6f;
        
        public RectTransform RectTransform { get; private set; }
        private CanvasGroup _canvasGroup;
        private Canvas _canvas;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvas = GetComponent<Canvas>();
        }

        public void PointerDown() => _canvasGroup.alpha = _alphaWhileDragged;

        public void BeginDrag()
        {
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = 0x7fff;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void EndDrag()
        {
            _canvas.overrideSorting = false;
            _canvasGroup.blocksRaycasts = true;
        }

        public void PointerUp() => _canvasGroup.alpha = 1f;
    }
}