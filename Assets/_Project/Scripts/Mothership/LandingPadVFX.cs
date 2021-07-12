using UnityEngine;
using UnityEngine.VFX;

namespace SpaceGame.Mothership {
    [AddComponentMenu("Shuttle Systems/Landing Pad VFX")]
    [RequireComponent(typeof(VisualEffect))]
    public class LandingPadVFX : MonoBehaviour {
        [SerializeField] private float _visibilityRadius = 200f;
        [SerializeField] private float _blendMargin = 50f;

        private VisualEffect _vfx;

        private void Awake() {
            _vfx = GetComponent<VisualEffect>();
            _vfx.enabled = false;
        }

        public void SetDistance(float distance)
        {
            if (distance > _visibilityRadius)
            {
                if (_vfx.enabled) _vfx.enabled = false;
                return;
            }

            if (!_vfx.enabled) _vfx.enabled = true;
            _vfx.SetFloat("Alpha", Mathf.InverseLerp(_visibilityRadius, _visibilityRadius - _blendMargin, distance));
        }

        public void Disable() => _vfx.enabled = false;

        private void OnDrawGizmosSelected() {
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _visibilityRadius);
            Gizmos.DrawWireSphere(position, _visibilityRadius-_blendMargin);
        }
    }
}
