﻿using System.Collections.Generic;
using SpaceGame.InventorySystem;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.ShuttleSystems
{
    public sealed class ShuttleConnector : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _enabledWhenSet = new List<GameObject>();
        [SerializeField] private UnityEvent<Shuttle> _connectShuttle = default;
        [SerializeField] private UnityEvent<Hull.Hull> _connectHull = default;
        [SerializeField] private UnityEvent<InertiaDampers.InertiaDampers> _connectInertiaDampers = default;
        [SerializeField] private UnityEvent<IInventory> _connectInventory = default;
        [SerializeField] private UnityEvent<MiningTool.MiningTool> _connectMiningTool = default;
        [SerializeField] private UnityEvent<PowerSystem.PowerSystem> _connectPowerSystem = default;
        [SerializeField] private UnityEvent<ResourceScanner.ResourceScanner> _connectResourceScanner = default;
        [SerializeField] private UnityEvent<Thrusters.Thrusters> _connectThrusters = default;
        

        private void Awake() => ShuttleSpawner.CurrentShuttle.Subscribe(OnNewShuttleSpawned);

        private void OnDestroy() => ShuttleSpawner.CurrentShuttle.Unsubscribe(OnNewShuttleSpawned);


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