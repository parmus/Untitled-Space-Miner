using System;
using System.Collections;
using DG.Tweening;
using SpaceGame.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame.UI
{
    public class LoadingScreen: Singleton<LoadingScreen>
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeSpeed = 0.5f;
        [SerializeField] private bool _initiallyFaded;
        [SerializeField] private int _skipFrames = 3;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup.alpha = _initiallyFaded ? 1 : 0;
            _canvasGroup.blocksRaycasts = _initiallyFaded;
            if (_initiallyFaded) StartCoroutine(FadeIn());
        }

        public IEnumerator FadeOut()
        {
            _canvasGroup.blocksRaycasts = true;
            yield return _canvasGroup.DOFade(1, _fadeSpeed).WaitForCompletion();
        }

        public IEnumerator FadeIn()
        {
            for (var i = 0; i < _skipFrames; i++)
            {
                yield return null;
            }
            var tween = _canvasGroup.DOFade(0, _fadeSpeed);
            tween.OnComplete(() => _canvasGroup.blocksRaycasts = false);
            yield return tween;
        }

        public void LoadGame() => StartCoroutine(CO_LoadGame());

        private IEnumerator CO_LoadGame()
        {
            yield return FadeOut();
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            yield return SessionManager.Instance.LoadGame("savegame");
            yield return FadeIn();
        }

        public void NewGame() => StartCoroutine(CO_NewGame());

        private IEnumerator CO_NewGame()
        {
            yield return FadeOut();
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            yield return SessionManager.Instance.NewGame();
            yield return FadeIn();
        }
    }
}
