using UnityEngine;

namespace SpaceGame.ShuttleSystems.InertiaDampers {
    [CreateAssetMenu(fileName = "New Inertia Dampers Configuration", menuName = "Game Data/Inertia Dampers Configuration", order = 1)]
    public class InertiaDamperUpgrade : ShuttleUpgrade, IInertiaDamperUpgrade {
        [SerializeField] private float _drag = 1f;
        [SerializeField] private float _angularDrag = 1f;

        public override string Name => name;
        public float Drag => _drag;
        public float AngularDrag => _angularDrag;

    }
}