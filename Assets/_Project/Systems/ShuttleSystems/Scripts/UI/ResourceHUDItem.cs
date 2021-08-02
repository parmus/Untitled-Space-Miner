using System;
using SpaceGame.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Resource HUD Item")]
    public class ResourceHUDItem : MonoBehaviour {
        public static Camera Camera = default;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _distanceLabel;
        [SerializeField] private TextMeshProUGUI _resourceTypeLabel;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private string _distanceFormat = "{0:0}m";

        private ResourceDeposit _resourceDeposit;
        private Transform _scannerOrigin;

        private Canvas _canvas;
        private RectTransform _canvasRectTransform;
        
        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        public void Bind(ResourceDeposit item, Transform scannerOrigin) {
            _resourceDeposit = item;
            _scannerOrigin = scannerOrigin;
            
            _healthBar.maxValue = _resourceDeposit.MaxHealth;
            _resourceDeposit.Health.Subscribe(OnResourceDepositDamaged);
            _resourceTypeLabel.text = _resourceDeposit.Type.name;
        }

        private void OnResourceDepositDamaged(float health) => _healthBar.value = health;

        private void Update() {
            if (_resourceDeposit == null)
            {
                Debug.LogError("this should not happen", this);
                Debug.Break();
                return;
            }

            var screenPos = Camera.WorldToScreenPoint(_resourceDeposit.Bounds.center);
            if (screenPos.z < Mathf.Epsilon) {
                _canvasGroup.alpha = 0;
                return;
            }

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRectTransform, screenPos, _canvas.worldCamera, out var pos);

            transform.position = pos;
            
            _canvasGroup.alpha = 1;
            var distance = Vector3.Distance(_scannerOrigin.position, _resourceDeposit.Bounds.center);
            _distanceLabel.text = string.Format(_distanceFormat, distance);
        }

        private void OnDestroy() => _resourceDeposit.Health.Unsubscribe(OnResourceDepositDamaged);
    }
}
