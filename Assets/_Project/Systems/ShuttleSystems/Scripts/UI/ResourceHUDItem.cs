using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Resource HUD Item")]
    public class ResourceHUDItem : MonoBehaviour {
        public static Camera Camera = default;

        [SerializeField] private CanvasGroup _canvasGroup = default;
        [SerializeField] private TMPro.TextMeshProUGUI _distanceLabel = default;
        [SerializeField] private TMPro.TextMeshProUGUI _resourceTypeLabel = default;
        [SerializeField] private Slider _healthBar = default;
        [SerializeField] private string _distanceFormat = "{0:0}m";

        private ResourceScanner.IResourceScannerItem _resourceScannerItem = null;

        public void Bind(ResourceScanner.IResourceScannerItem item) {
            _resourceScannerItem = item;
            var resourceDeposit = _resourceScannerItem.ResourceDeposit;
            _healthBar.maxValue = resourceDeposit.MaxHealth;

            resourceDeposit.Health.OnChange += OnResourceDepositDamaged;
            OnResourceDepositDamaged(resourceDeposit.Health.Value);

            _resourceTypeLabel.text = resourceDeposit.Type.name;
            _resourceScannerItem.OnDestroy += OnResourceScannerItemDestroy;
        }

        private void OnResourceDepositDamaged(float health) => _healthBar.value = health;

        private void OnResourceScannerItemDestroy() => Destroy(gameObject);

        private bool IsBehindCamera => transform.position.z < Mathf.Epsilon;

        private void Update() {
            if (_resourceScannerItem == null)
            {
                Debug.LogError("this should not happen", this);
                Debug.Break();
                return;
            }

            transform.position = Camera.WorldToScreenPoint(_resourceScannerItem.Transform.position);
            if (IsBehindCamera || !_resourceScannerItem.InRange) {
                _canvasGroup.alpha = 0;
                return;
            }

            _canvasGroup.alpha = 1;
            _distanceLabel.text = string.Format(_distanceFormat, _resourceScannerItem.Distance);
        }

        private void OnDestroy() {
            _resourceScannerItem.OnDestroy -= OnResourceScannerItemDestroy;
            _resourceScannerItem.ResourceDeposit.Health.OnChange -= OnResourceDepositDamaged;
        }
    }
}