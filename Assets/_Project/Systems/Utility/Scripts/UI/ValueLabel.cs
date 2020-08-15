using UnityEngine;

namespace SpaceGame.Utility.UI {
    public class ValueLabel : MonoBehaviour {
        [Header("UI elements")]
        [SerializeField] private TMPro.TextMeshProUGUI _label = default;

        [Header("Settings")]
        [SerializeField] private string _format = "<b>Value:</b> {0:P0}";
        [SerializeField] private float _value = 1f;

        public float Value {
            get => _value;
            set {
                _value = value;
                UpdateUI();
            }
        }

        private void UpdateUI() {
            _label.text = string.Format(_format, _value);
        }

        private void Reset() {
            _label = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            UpdateUI();
        }

        private void OnValidate() {
            UpdateUI();
        }
    }
}
