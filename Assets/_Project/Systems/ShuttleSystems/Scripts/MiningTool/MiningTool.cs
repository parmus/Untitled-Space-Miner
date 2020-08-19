using SpaceGame.AsteroidSystem;
using SpaceGame.Message_UI_System;
using SpaceGame.ShuttleSystems.MiningTool.VFX;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.MiningTool {
    [AddComponentMenu("ShuttleSystems/MiningTool")]
    public class MiningTool : MonoBehaviour {
        #region Serialized fields
        [SerializeField] private Transform _origin = default;
        [SerializeField] private DefaultMiningTool _defaultMiningTool = new DefaultMiningTool();
        #endregion

        #region Properties

        public IReadonlyObservable<bool> InRange => _inRange;
        public readonly IObservable<IMiningToolConfiguration> Upgrade = new Observable<IMiningToolConfiguration>();
        #endregion

        #region Private properties and variables
        private readonly Observable<bool> _inRange = new Observable<bool>();
        private bool Use => _shuttle.ShuttleControls.Fire;
        private IMiningToolConfiguration CurrentMiningTool => Upgrade.Value ?? _defaultMiningTool;
        private MiningToolVFX _vfx = default;
        private ResourceDeposit _target = null;
        private Shuttle _shuttle;
        #endregion

        private void Awake() {
            _shuttle = GetComponent<Shuttle>();
            Upgrade.OnChange += upgrade => {
                if (_vfx) Destroy(_vfx.gameObject);
                _vfx = Instantiate(CurrentMiningTool.VFXPrefab, _origin);
            };

            SelectUnderCursor.Target.OnChange += OnTargetChange;
            OnTargetChange(SelectUnderCursor.Target.Value);
        }

        private void Start() {
            if (!_vfx) _vfx = Instantiate(CurrentMiningTool.VFXPrefab, _origin);
        }

        private void OnDestroy() => SelectUnderCursor.Target.OnChange -= OnTargetChange;

        private void OnTargetChange(GameObject target) => _target = target ? target.GetComponentInParent<ResourceDeposit>() : null;

        private void OnDisable() {
            UpdateInRange();
            UpdateVFX();
        }

        private void UpdateInRange() {
            _inRange.Value = enabled && _target && Vector3.Distance(_origin.position, _target.transform.position) < CurrentMiningTool.Range;
        }

        private void UpdateVFX() {
            if (!_vfx) return;
            var isFiringLaser = enabled && Use && !_shuttle.PowerSystem.IsEmpty;
            _vfx.IsMining = isFiringLaser;
            _vfx.TargetPosition = SelectUnderCursor.Hit;
            _vfx.IsHitting = isFiringLaser && _inRange.Value;
        }

        private void Update() {
            UpdateInRange();
            UpdateVFX();
            if (!Use || _shuttle.PowerSystem.IsEmpty) return;
            _shuttle.PowerSystem.Charge -= CurrentMiningTool.PowerConsumption * Time.deltaTime;
            if (!_inRange.Value || !_target.Damage(CurrentMiningTool.Strength * Time.deltaTime)) return;
            var resourceAcquired = _shuttle.Storage.Inventory.Add(_target.Type, _target.Amount);
            Broker.Push(resourceAcquired < 1
                ? "Shuttle inventory full!"
                : $"+{resourceAcquired} {_target.Type.Name}");
        }

        private void Reset() {
            _origin = transform;
        }

        [System.Serializable]
        private class DefaultMiningTool: IMiningToolConfiguration {
            [Header("Laser parameters")]
            [SerializeField] private float _strength = 10f;
            [SerializeField] private float _range = 50f;
            [SerializeField] private float _powerConsumption = 2f;

            [Header("VFX")]
            [SerializeField] private MiningToolVFX _vfxPrefab = default;

            public float Strength => _strength;
            public float Range => _range;
            public float PowerConsumption => _powerConsumption;
            public MiningToolVFX VFXPrefab => _vfxPrefab;
        }
    }
}