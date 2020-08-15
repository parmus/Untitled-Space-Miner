using UnityEngine;

namespace SpaceGame.Utility.UI {
    public class ProgressBar : MonoBehaviour {
        [Header("UI elements")]
        [SerializeField] private UnityEngine.UI.Image _bar = default;

        [Header("Settings")]
        [SerializeField] private Gradient _barColor = default;
        [Space]

        [SerializeField][Delayed] private float _min = 0f;
        [SerializeField][Delayed] private float _max = 1f;
        [SerializeField] private float _value = 1f;

        public float Value {
            get => _value;
            set {
                _value = Mathf.Clamp(value, _min, _max);
                UpdateUI();
            }
        }

        public float Max {
            get => _max;
            set {
                _max = Mathf.Max(_min, value);
                _value = Mathf.Clamp(value, _min, _max);
                UpdateUI();
            }
        }

        public float Min {
            get => _min;
            set {
                _min = Mathf.Min(_max, value);
                _value = Mathf.Clamp(value, _min, _max);
                UpdateUI();
            }
        }

        private void Reset() {
            _bar = GetComponentInChildren<UnityEngine.UI.Image>();
            UpdateUI();
        }

        private void UpdateUI() {
            var filled = Mathf.InverseLerp(_min, _max, _value);
            _bar.fillAmount = filled;
            _bar.color = _barColor.Evaluate(filled);
        }

        private void OnValidate() {
            _max = Mathf.Max(_min, _max);
            _value = Mathf.Clamp(_value, _min, _max);
            if (_bar != null) UpdateUI();
        }
    }
}