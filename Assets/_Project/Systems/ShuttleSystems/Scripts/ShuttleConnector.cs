using System.Collections.Generic;
using SpaceGame.InventorySystem;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems
{
    public sealed class ShuttleConnector : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _enabledWhenSet = new List<GameObject>();
        [SerializeField] private UnityEvent<Shuttle> _connectShuttle;
        [SerializeField] private UnityEvent<Hull.Hull> _connectHull;
        [SerializeField] private UnityEvent<InertiaDampers.InertiaDampers> _connectInertiaDampers;
        [SerializeField] private UnityEvent<IInventory> _connectInventory;
        [SerializeField] private UnityEvent<MiningTool.MiningTool> _connectMiningTool;
        [SerializeField] private UnityEvent<PowerSystem.PowerSystem> _connectPowerSystem;
        [SerializeField] private UnityEvent<ResourceScanner.ResourceScanner> _connectResourceScanner;
        [SerializeField] private UnityEvent<Thrusters.Thrusters> _connectThrusters;
        [SerializeField] private ShuttleAnchor _shuttleAnchor;

        private void Awake() => _shuttleAnchor.Subscribe(OnNewShuttleSpawned);

        private void OnDestroy() => _shuttleAnchor.Unsubscribe(OnNewShuttleSpawned);


        private void OnNewShuttleSpawned(Shuttle shuttle)
        {
            _enabledWhenSet.ForEach(go => go.SetActive(shuttle != null));
            _connectShuttle.Invoke(shuttle);
            _connectHull.Invoke(shuttle ? shuttle.Hull : null);
            _connectInertiaDampers.Invoke(shuttle ? shuttle.InertiaDampers : null);
            _connectInventory.Invoke(shuttle ? shuttle.Storage.Inventory : null);
            _connectMiningTool.Invoke(shuttle ? shuttle.MiningTool : null);
            _connectPowerSystem.Invoke(shuttle ? shuttle.PowerSystem : null);
            _connectResourceScanner.Invoke(shuttle ? shuttle.ResourceScanner : null);
            _connectThrusters.Invoke(shuttle ? shuttle.Thrusters : null);
        }
    }
}
