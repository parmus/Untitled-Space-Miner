using UnityEngine;

namespace SpaceGame.ShuttleSystems.MiningTool.VFX {
    public abstract class MiningToolVFX: MonoBehaviour {
        public virtual bool IsMining {get; set; }
        public virtual Vector3 TargetPosition { get; set; }
        public virtual bool IsHitting { get; set; }
    }
}