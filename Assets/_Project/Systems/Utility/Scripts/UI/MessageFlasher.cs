using DG.Tweening;
using UnityEngine;

namespace SpaceGame.Utility.UI
{
    public class MessageFlasher : MonoBehaviour {
        #region Serialized fields
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private string _message = default;
        [SerializeField] private float _blinkingSpeed = 0.1f;
        [SerializeField] private Ease _ease = Ease.Unset;
        [SerializeField] private LoopType _loopType = LoopType.Yoyo;
        [SerializeField] private int _loops = -1;
        #endregion

        #region Properties
        public Color Color {
            get => _color;
            set {
                _color = value;
                _frame.color = _color;
                _messageField.color = _color;
            }
        }
        public string Message {
            get => _message;
            set {
                _message = value;
                _messageField.text = _message;
                enabled = _message != "";
            }
        }
        public float BlinkingSpeed {
            get => _blinkingSpeed;
            set {
                if (_blinkingSpeed == value) return;
                _blinkingSpeed = value;
                RestartTween();
            }
        }
        public Ease Ease {
            get => _ease;
            set {
                if (_ease == value) return;
                _ease = value;
                RestartTween();
            }
        }
        public LoopType LoopType {
            get => _loopType;
            set {
                if (_loopType == value) return;
                _loopType = value;
                RestartTween();
            }
        }
        public int Loops {
            get => _loops;
            set {
                if (_loops == value) return;
                _loops = value;
                RestartTween();
            }
        }
        #endregion

        private void RestartTween()
        {
            if (!enabled) return;
            _canvasGroup.DOKill();
            _canvasGroup.alpha = _ease != Ease.Unset ? 0 : 1;
            if (_ease != Ease.Unset) _canvasGroup.DOFade(1, _blinkingSpeed).SetLoops(_loops, _loopType).SetEase(_ease);
        }

        #region Private variables
        private TMPro.TMP_Text _messageField;
        private UnityEngine.UI.Image _frame;
        private CanvasGroup _canvasGroup;
        #endregion


        private void Awake() {
            _messageField = GetComponentInChildren<TMPro.TMP_Text>();
            _frame = GetComponentInChildren<UnityEngine.UI.Image>();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        private void OnEnable() {
            RestartTween();
        }

        private void OnDisable() {
            _canvasGroup.DOKill();
            _canvasGroup.alpha = 0;
        }

        private void OnValidate() {
            var frame = GetComponentInChildren<UnityEngine.UI.Image>();
            if (frame != null) {
                frame.color = _color;
            }

            var messageField = GetComponentInChildren<TMPro.TMP_Text>();
            if (messageField == null) return;
            messageField.color = _color;
            messageField.alpha = 1f;
            messageField.text = _message;
        }

        private void Reset() {
            var messageField = GetComponentInChildren<TMPro.TMP_Text>();
            if (messageField != null) {
                _message = messageField.text;
            }

            var frame = GetComponentInChildren<UnityEngine.UI.Image>();
            if (frame != null) {
                _color = frame.color;
            }
        }
    }
}
