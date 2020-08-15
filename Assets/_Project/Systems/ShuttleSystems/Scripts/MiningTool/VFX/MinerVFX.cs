using UnityEngine;

namespace SpaceGame.ShuttleSystems.MiningTool.VFX {
    [AddComponentMenu("Shuttle Systems/Mining Tool/Mining Tool VFX")]
    public class MinerVFX: MiningToolVFX {
        [SerializeField] private ParticleSystem _hitSparks = default;
        [SerializeField] private LineRenderer _laser = default;

        public override bool IsMining {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        private void Awake() {
            gameObject.SetActive(false);
        }

        private void Update() {
            if (IsMining) {
                _laser.SetPosition(1, transform.InverseTransformPoint(TargetPosition));
                if (IsHitting) {
                    var hitSparksTransform = _hitSparks.transform;
                    hitSparksTransform.position = TargetPosition;
                    hitSparksTransform.rotation = transform.rotation;
                    if (!_hitSparks.isPlaying) _hitSparks.Play();
                } else {
                    if (_hitSparks.isPlaying) _hitSparks.Stop();
                }
            } else {
                if (_hitSparks.isPlaying) _hitSparks.Stop();
            }
        }

        private void Reset() {
            _hitSparks = GetComponentInChildren<ParticleSystem>();
            _laser = GetComponentInChildren<LineRenderer>();
        }
    }
}