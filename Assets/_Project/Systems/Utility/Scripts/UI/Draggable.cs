using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceGame.Utility.UI
{
    public class Draggable: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private DraggableAvatar _draggableAvatar;
        
        private float _scaleFactor;
        private Vector3 _oldPosition;

        private void Awake() => _scaleFactor = GetComponentInParent<Canvas>().scaleFactor;

        public void OnPointerDown(PointerEventData eventData) => _draggableAvatar.PointerDown();

        public void OnBeginDrag(PointerEventData eventData)
        {
            _oldPosition = _draggableAvatar.RectTransform.anchoredPosition;
            _draggableAvatar.BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _draggableAvatar.RectTransform.anchoredPosition += eventData.delta / _scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _draggableAvatar.RectTransform.anchoredPosition = _oldPosition;
            _draggableAvatar.EndDrag();
        }

        public void OnPointerUp(PointerEventData eventData) => _draggableAvatar.PointerUp();
    }
}
