using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceGame.Utility
{
    [RequireComponent(typeof(Volume))]
    public class VolumeDriver : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
        
        private Volume _volume;

        private void Awake() => _volume = GetComponent<Volume>();

        public void SetWeight(float weight) => _volume.weight = _curve.Evaluate(weight);
    }
}