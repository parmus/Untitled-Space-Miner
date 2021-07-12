using DG.Tweening;
using UnityEngine;

namespace SpaceGame.Utility.UI {
    public class FlashImage : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Image _image;
        [Header("Tween settings")]
        [SerializeField] private Color _flashColor = Color.yellow;
        [SerializeField] private float _flashDuration = 1f;
        [SerializeField] private Ease _ease = Ease.InQuad;

        public void Execute() {
            _image.DOComplete();
            _image.DOColor(_flashColor, _flashDuration / 2).SetLoops(2, LoopType.Yoyo).SetEase(_ease);
        }
    }
}
