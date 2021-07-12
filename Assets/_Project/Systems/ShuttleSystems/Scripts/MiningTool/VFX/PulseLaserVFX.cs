using UnityEngine;
using UnityEngine.VFX;

namespace SpaceGame.ShuttleSystems.MiningTool.VFX {
    [AddComponentMenu("Shuttle Systems/Mining Tool/Pulse Laser VFX")]
    [RequireComponent(typeof(VisualEffect))]
    [RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
    public class PulseLaserVFX: MiningToolVFX {
        private VisualEffect _vfx;
        private FMODUnity.StudioEventEmitter _emitter;

        private void Awake() {
            _vfx = GetComponent<VisualEffect>();
            _emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }

        private void Update() {
            _vfx.SetBool("Enabled", IsMining);
            _vfx.SetVector3("Target", TargetPosition);
            _vfx.SetBool("Is hitting", IsHitting);

            if (IsMining) {
                if (!_emitter.IsPlaying()) _emitter.Play();
                _emitter.SetParameter("Hitting", IsHitting ? 1 : 0);
            } else {
                if (_emitter.IsPlaying()) _emitter.Stop();
            }
        }

        private void OnDestroy() => _emitter.Stop();
    }
}
