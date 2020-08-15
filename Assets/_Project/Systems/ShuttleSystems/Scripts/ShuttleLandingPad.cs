using UnityEngine;

namespace SpaceGame.ShuttleSystems
{
    public class ShuttleLandingPad : MonoBehaviour
    {
        [Header("Landing Pad")]
        [SerializeField] private float _landingPadRadius = 36;

        private Shuttle _shuttle;
        private Transform _transform;

        private void Awake() => _transform = transform;

        public void SetShuttle(Shuttle shuttle)
        {
            _shuttle = shuttle;
        }

        private void Update()
        {
            if (!_shuttle) return;

            var shuttlePosition = _shuttle.transform.position;
            var landingPadPosition = _transform.position;
            
            var distance = Vector3.Distance(shuttlePosition, landingPadPosition);
            var readyToLand = distance < _landingPadRadius && shuttlePosition.y > landingPadPosition.y;
            _shuttle.LandingPad = readyToLand ? _transform : null;
        }
        
        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, _landingPadRadius);
        }
    }
}