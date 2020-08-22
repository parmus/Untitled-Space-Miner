using SpaceGame.ShuttleSystems;
using SpaceGame.ShuttleSystems.ShuttleStates;
using UnityEngine;

namespace SpaceGame.Mothership
{
    [RequireComponent(typeof(LandingPadVFX))]
    public class LandingPad : MonoBehaviour
    {
        [SerializeField] private float _landingPadRadius = 36;
        [SerializeField] private float _activationMargin = 20f;

        private LandingPadVFX _landingPadVFX;
        private Shuttle _shuttle;
        private Transform _transform;
        private bool _landingZoneEnabled = false;

        private void Awake()
        {
            _landingPadVFX = GetComponent<LandingPadVFX>();
            _transform = transform;
        }

        public void SetShuttle(Shuttle shuttle)
        {
            if (_shuttle != null) _shuttle.CurrentState.Unsubscribe(OnShuttleStateChange);
            _shuttle = shuttle;
            if (_shuttle != null) _shuttle.CurrentState.Subscribe(OnShuttleStateChange);
        }

        private void OnDestroy()
        {
            if (_shuttle != null) _shuttle.CurrentState.Unsubscribe(OnShuttleStateChange);
        }

        private void OnShuttleStateChange(ShuttleStateMachine.State state)
        {
            if (!(state is LandedState)) return;
            _landingZoneEnabled = false;
            _landingPadVFX.Disable();
        }

        private void Update()
        {
            if (!(_shuttle && _shuttle.CurrentState.Value is FlyingState)) return;

            var shuttlePosition = _shuttle.transform.position;
            var landingPadPosition = _transform.position;

            var distance = Vector3.Distance(shuttlePosition, landingPadPosition);
            if (!_landingZoneEnabled && distance > _landingPadRadius + _activationMargin) _landingZoneEnabled = true;
            if (!_landingZoneEnabled) return;

            _landingPadVFX.SetDistance(distance);
            var readyToLand = distance < _landingPadRadius && shuttlePosition.y > landingPadPosition.y;
            if (readyToLand) _shuttle.Land(_transform);
        }
        
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _landingPadRadius);
            Gizmos.DrawWireSphere(position, _landingPadRadius + _activationMargin);
        }
    }
}