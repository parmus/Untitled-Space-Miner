using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace SpaceGame.Utility.UI
{
    [RequireComponent(typeof(Canvas))]
    public class TooltipUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _text = default;
        [SerializeField] private Image  _frame = default;
        [SerializeField] private Vector2 _cursorOffset = new Vector2(0, 0);

        #region Private variables
        private static TooltipUI _instance;

        private RectTransform _frameRect;
        private Canvas _rootCanvas;
        private RectTransform _rootCanvasRect;
        private Vector2 _padding;
        #endregion
        

        public static void SetText(string text)
        {
            Assert.IsNotNull(_instance);
            _instance.SetText_Instance(text);
        }

        public static void Hide() {
            Assert.IsNotNull(_instance);
            _instance.Hide_Instance();
        }

        private void Awake()
        {
            var textRect = _text.rectTransform;
            _padding = textRect.offsetMin - textRect.offsetMax;

            _frameRect = _frame.rectTransform;

            _rootCanvas = GetComponent<Canvas>().rootCanvas;
            _rootCanvasRect = _rootCanvas.GetComponent<RectTransform>();
            
            _instance = this;
            Hide();
        }

        private void SetText_Instance(string text)
        {
            gameObject.SetActive(true);
            _text.text = text;
            _text.ForceMeshUpdate();
            _frameRect.sizeDelta = _text.GetRenderedValues(false) + _padding;
            UpdatePosition();
        }

        private void Hide_Instance() => gameObject.SetActive(false);

        private void UpdatePosition()
        {
            Vector2 mousePosition = Input.mousePosition;
            var localPoint = (mousePosition + _cursorOffset) / _rootCanvas.scaleFactor;

            localPoint.x = Mathf.Clamp(localPoint.x, 0, _rootCanvasRect.rect.width - _frameRect.rect.width);
            localPoint.y = Mathf.Clamp(localPoint.y, _frameRect.rect.height, _rootCanvasRect.rect.height);
            
            _frameRect.anchoredPosition = localPoint;
        }

        private void Update() => UpdatePosition();
    }
}
