using System;
using UnityEngine;
using UnityEngine.VFX;

namespace SpaceGame.ShuttleSystems {
    [AddComponentMenu("Shuttle Systems/Landing Pad VFX")]
    [RequireComponent(typeof(VisualEffect))]
    public class LandingPadVFX : MonoBehaviour {
        [SerializeField] private float _radius = 200f;
        [SerializeField] private float _range = 50f;

        private VisualEffect _landingPadVFX = default;
        private Transform _shuttleTransform = default;
        private Shuttle _shuttle = default;

        public void SetShuttle(Shuttle shuttle) {
            if (_shuttle) _shuttle.CurrentState.Unsubscribe(OnShuttleChangeState);
            _shuttle = shuttle;
            
            if (_shuttle) {
                _shuttleTransform = shuttle.transform;
                _shuttle.CurrentState.Subscribe(OnShuttleChangeState);
            } else {
                _shuttleTransform = null;
            }
        }

        private void OnDestroy()
        {
            if (_shuttle) _shuttle.CurrentState.Unsubscribe(OnShuttleChangeState);
        }

        private void Awake() {
            _landingPadVFX = GetComponent<VisualEffect>();
            _landingPadVFX.enabled = false;
        }

        private void OnShuttleChangeState(ShuttleStates.FSM.State state) {
            if (_landingPadVFX == null) return;
            if (state == null || state is ShuttleStates.LandedState) _landingPadVFX.enabled = false;
        }

        private void Update() {
            if (!_shuttle) return;

            var distance = Vector3.Distance(_shuttleTransform.position, transform.position);

            if (!_landingPadVFX.enabled) {
                if (_shuttle.CurrentState.Value is ShuttleStates.FlyingState && distance > _radius-_range) {
                    _landingPadVFX.enabled = true;
                }
            }

            _landingPadVFX.SetFloat("Alpha", Mathf.InverseLerp(_radius, _radius-_range, distance));
        }

        private void OnDrawGizmosSelected() {
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _radius);
            Gizmos.DrawWireSphere(position, _radius-_range);
        }
    }
}
