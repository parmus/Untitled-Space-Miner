using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SpaceGame.Message_UI_System {
    public class LogHUD: MonoBehaviour {
        [SerializeField] private LogEntry _logEntryPrefab = default;
        [SerializeField] private RectTransform _entries = default;

        [Header("Animation control")]
        [SerializeField] private float _fadeInTime = 0.5f;
        [SerializeField] private float _fadeOutTime = 0.5f;
        [SerializeField] private float _scrollTime = 0.5f;
        [SerializeField] private float _defaultDuration = 3f;

        private void Start() {
            StartCoroutine(PopNext());
        }

        public void Push(string message) {
            Push(message, _defaultDuration);
        }

        public void Push(string message, float duration) {
            var logEntry = Instantiate(_logEntryPrefab, _entries);
            logEntry.SetMessage(message);
            DOTween.To(
                () => logEntry.Alpha,
                alpha => logEntry.Alpha = alpha,
                1f,
                _fadeInTime
            );
        }

        private IEnumerator PopNext() {
            while (true) {
                yield return new WaitUntil(() => _entries.childCount > 0);

                var head = _entries.GetChild(0).GetComponent<LogEntry>();
                yield return new WaitForSeconds(head.ExpireIn);

                //var seq = DOTween.Sequence();
                //seq.AppendInterval(head.ExpireIn);
                yield return DOTween.To(
                    () => head.Alpha, alpha => head.Alpha = alpha,
                    0f, _fadeOutTime
                ).WaitForCompletion();
                yield return DOTween.To(
                    () => _entries.offsetMax.y, top => _entries.offsetMax = new Vector2(_entries.offsetMax.x, top),
                    head.Height, _scrollTime
                ).WaitForCompletion();
                //yield return seq.WaitForCompletion();
                Destroy(_entries.GetChild(0).gameObject);
                _entries.offsetMax = new Vector2(_entries.offsetMax.x, 0);
                yield return null;
            }
        }

        private void OnEnable() => Broker.OnNewMessage += Push;

        private void OnDisable() => Broker.OnNewMessage -= Push;
    }
}