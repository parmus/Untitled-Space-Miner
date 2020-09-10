﻿using SpaceGame.InventorySystem;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class SessionConnector : MonoBehaviour
    {
        [SerializeField] private UnityEvent<IInventory> _onInventoryChange = default;

        private void Start() => _onInventoryChange.Invoke(SessionManager.Instance.Inventory);
    }
}