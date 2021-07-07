using SpaceGame.ShuttleSystems.ResourceScanner;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Resource HUD")]
    public class ResourceHUD : MonoBehaviour {
        [SerializeField] private ResourceHUDItem _iconPrefab = default;
        
        private ResourceScanner.ResourceScanner _resourceScanner = default;

        public void SetMainCamera(Camera camera) => ResourceHUDItem.Camera = camera;

        public void SetResourceScanner(ResourceScanner.ResourceScanner shuttle) {
            if (_resourceScanner) {
                _resourceScanner.OnEnter -= OnResourceEnter;
                transform.DestroyAllChildren();
            }
            _resourceScanner = shuttle;
            if (!_resourceScanner) return;
            _resourceScanner.OnEnter += OnResourceEnter;

            foreach(var r in _resourceScanner.InRange) {
                OnResourceEnter(r);
            }
        }

        private void OnDestroy() {
            if (_resourceScanner != null) _resourceScanner.OnEnter -= OnResourceEnter;
            transform.DestroyAllChildren();
        }

        private void OnResourceEnter(IResourceScannerItem item) => Instantiate(_iconPrefab, transform).Bind(item);
    }
}
