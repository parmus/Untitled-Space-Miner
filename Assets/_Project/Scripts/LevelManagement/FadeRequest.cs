using System;
using UnityEngine;

namespace SpaceGame.LevelManagement
{    
    [CreateAssetMenu(fileName = "Fade Request", menuName = "Game Events/Fade Request Event")]
    public class FadeRequest : ScriptableObject
    {
        public event Action<float, bool> OnEvent;

        public void FadeIn(float duration) => Fade(duration, true);
        public void FadeOut(float duration) => Fade(duration, false);

        private void Fade(float duration, bool fadeIn) => OnEvent?.Invoke(duration, fadeIn);
    }
}
