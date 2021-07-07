using DG.Tweening;
using SpaceGame.Utility.GameEvents;
using UnityEngine.Rendering;
using UnityEngine;

namespace SpaceGame.LevelManagement
{
    [RequireComponent(typeof(Volume))]
    public class VolumeFader : MonoBehaviour
    {
        [SerializeField] private FloatGameEvent _fadeOutRequest;
        [SerializeField] private FloatGameEvent _fadeInRequest;
        
        private Volume _volume;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.weight = 0f;
        }

        private void OnEnable()
        {
            _fadeOutRequest.OnEvent += FadeOut;
            _fadeInRequest.OnEvent += FadeIn;
        }

        private void OnDisable()
        {
            _fadeOutRequest.OnEvent += FadeOut;
            _fadeInRequest.OnEvent += FadeIn;
        }

        private void FadeOut(float duration) => DOTween.To(() => _volume.weight, w => _volume.weight = w, 1, duration);

        private void FadeIn(float duration) => DOTween.To(() => _volume.weight, w => _volume.weight = w, 0, duration);
    }
}
