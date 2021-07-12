using System.Collections;
using System.Linq;
using SpaceGame.ShuttleSystems;
using UnityEngine;

namespace SpaceGame.Mothership
{
    public class ShuttleMaintenance : MonoBehaviour {
        [SerializeField] private float _inventoryTransferDelay = 1f;
        [SerializeField] private float _chargingSpeed = 10f;

        private Shuttle _shuttle;
        private Coroutine _emptyInventory;
        private bool _charging;

        public void SetShuttle(Shuttle shuttle) {
            if (_shuttle) {
                _shuttle.CurrentState.Unsubscribe(OnShuttleStateChange);
                if (_emptyInventory != null) _shuttle.StopCoroutine(_emptyInventory);
            }
            _shuttle = shuttle;
            if (!_shuttle) return;
            _shuttle.CurrentState.Subscribe(OnShuttleStateChange);
        }

        private void OnDestroy()
        {
            if (_shuttle) _shuttle.CurrentState.Unsubscribe(OnShuttleStateChange);
        }

        private void OnShuttleStateChange(ShuttleSystems.ShuttleStates.ShuttleStateMachine.State state) {
            _charging = state is ShuttleSystems.ShuttleStates.LandedState;
            if (state is ShuttleSystems.ShuttleStates.LandedState) {
                _emptyInventory = StartCoroutine(CO_EmptyInventory());
            } else {
                if (_emptyInventory != null) _shuttle.StopCoroutine(_emptyInventory);
            }
        }

        private void Update() {
            if (_charging) {
                _shuttle.PowerSystem.Recharge(_chargingSpeed * Time.deltaTime);
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
