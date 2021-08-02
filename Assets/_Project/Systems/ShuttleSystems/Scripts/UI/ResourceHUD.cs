using System.Collections.Generic;
using SpaceGame.Core;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Resource HUD")]
    public class ResourceHUD : MonoBehaviour {
        [SerializeField] private ResourceHUDItem _iconPrefab;
        
        private ResourceScanner.ResourceScanner _resourceScanner;

        public void SetMainCamera(Camera camera) => ResourceHUDItem.Camera = camera;

        public void SetResourceScanner(ResourceScanner.ResourceScanner resourceScanner) {
            if (_resourceScanner) {
                _resourceScanner.OnEnter -= OnResourceEnter;
                _resourceScanner.OnLeave -= OnResourceLeave;
                transform.DestroyAllChildren();
                _hudItems.Clear();
            }
            
            _resourceScanner = resourceScanner;
            if (!_resourceScanner) return;
            
            _resourceScanner.OnEnter += OnResourceEnter;
            _resourceScanner.OnLeave += OnResourceLeave;

            foreach(var r in _resourceScanner.InRange) {
                OnResourceEnter(r);
            }
        }

        private readonly Dictionary<ResourceDeposit, ResourceHUDItem> _hudItems = new Dictionary<ResourceDeposit, ResourceHUDItem>();

        private void OnResourceEnter(ResourceDeposit resourceDeposit)
        {
            var item = Instantiate(_iconPrefab, transform);
            _hudItems[resourceDeposit] = item;
            item.Bind(resourceDeposit, _resourceScanner.Origin);

            resourceDeposit.OnDestroy += OnResourceLeave;
        }

        private void OnResourceLeave(ResourceDeposit resourceDeposit)
        {
            if (!_hudItems.TryGetValue(resourceDeposit, out var item)) return;
            resourceDeposit.OnDestroy -= OnResourceLeave;
            Destroy(item.gameObject);
            _hudItems.Remove(resourceDeposit);
        }

        private void OnDestroy()
        {
            if (!_resourceScanner) return;
            _resourceScanner.OnEnter -= OnResourceEnter;
            _resourceScanner.OnLeave -= OnResourceLeave;
            transform.DestroyAllChildren();
            _hudItems.Clear();
        }
    }
}
