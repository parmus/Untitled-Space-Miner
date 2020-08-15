using System.Collections;
using System.Linq;
using UnityEngine;

namespace SpaceGame.ShuttleSystems
{
    public class ShuttleMaintenance : MonoBehaviour {
        [SerializeField] private float _inventoryTransferDelay = 1f;
        [SerializeField] private float _chargingSpeed = 10f;

        private Shuttle _shuttle = default;
        private Coroutine _emptyInventory;
        private bool _charging = false;

        public void SetShuttle(Shuttle shuttle) {
            if (_shuttle) {
                _shuttle.CurrentState.OnChange -= OnShuttleStateChange;
                if (_emptyInventory != null) _shuttle.StopCoroutine(_emptyInventory);
            }
            _shuttle = shuttle;
            if (!_shuttle) return;
            _shuttle.CurrentState.OnChange += OnShuttleStateChange;
            OnShuttleStateChange(_shuttle.CurrentState.Value);
        }

        private void OnShuttleStateChange(ShuttleStates.FSM.State state) {
            _charging = state is ShuttleStates.LandedState;
            if (state is ShuttleStates.LandedState) {
                _emptyInventory = StartCoroutine(CO_EmptyInventory());
            } else {
                if (_emptyInventory != null) _shuttle.StopCoroutine(_emptyInventory);
            }
        }

        private void Update() {
            if (_charging) {
                _shuttle.PowerSystem.Charge += _chargingSpeed * Time.deltaTime;
            }
        }

        private IEnumerator CO_EmptyInventory() {
            var inventoryTransferDelay = new WaitForSeconds(_inventoryTransferDelay);
            foreach(var stack in _shuttle.Storage.Inventory.Reverse()) {
                if (!stack.Type) continue;
                yield return inventoryTransferDelay;
                SessionManager.Instance.Inventory.Move(stack);
            }
        }
    }
}