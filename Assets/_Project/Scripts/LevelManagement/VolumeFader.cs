using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine;

namespace SpaceGame.LevelManagement
{
    [RequireComponent(typeof(Volume))]
    public class VolumeFader : MonoBehaviour
    {
        [SerializeField] private FadeRequest _fadeRequest;
        [SerializeField] private Ease _ease = Ease.Linear;
        
        private Volume _volume;
        private Tween _tween;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.weight = 0f;
        }

        private void OnEnable() => _fadeRequest.OnEvent += Fade;

        private void OnDisable() => _fadeRequest.OnEvent -= Fade;

        private void Fade(float duration, bool fadeIn)
        {
            _tween?.Kill();
            _tween = fadeIn ? DOTween.To(w => _volume.weight = w, 1f, 0f, duration) : DOTween.To(w => _volume.weight = w, 0f, 1f, duration);
            _tween.SetEase(_ease);
        } 
    }
}
