using DG.Tweening;
using UnityEngine;

namespace SpaceGame.Utility.UI {
    public class ScaleRect : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform = default;
        [Header("Tween settings")]
        [SerializeField] private float _scale = 1.2f;
        [SerializeField] private float _scaleDuration = 1f;
        [SerializeField] private Ease _ease = Ease.InQuad;

        private Vector3 _scaleVector;

        private void Awake() {
            _scaleVector = new Vector3(_scale, _scale, _scale);
        }

        public void Execute() {
            _rectTransform.DOComplete();
            _rectTransform.DOScale(_scaleVector, _scaleDuration / 2).SetLoops(2, LoopType.Yoyo).SetEase(_ease);
        }
    }
}