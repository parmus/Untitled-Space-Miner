using TMPro;
using UnityEngine;

namespace SpaceGame.Message_UI_System {
    [RequireComponent(typeof(TMP_Text))]
    public class LogEntry : MonoBehaviour {
        private TMP_Text _text;
        private RectTransform _rect;

        private float _duration;
        private float _start;

        public float ExpireIn => Mathf.Max(_start + _duration - Time.time, 0f);
        public float Height => _rect.rect.height;

        public float Alpha {
            get => _text.alpha;
            set => _text.alpha = value;
        }

        private void Awake() {
            _rect = GetComponent<RectTransform>();
            _text = GetComponent<TMP_Text>();
            Alpha = 0;
        }

        public void SetMessage(string message, float duration = 5f) {
            _text.text = message;
            _duration = duration;
            _start = Time.time;
        }
    }
}