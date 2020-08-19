using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.ShuttleSystems.UI {
    [AddComponentMenu("Shuttle Systems/UI/Mining Aim HUD")]
    public class MiningAimHUD: MonoBehaviour {
        [SerializeField] private Image _icon = default;
        [SerializeField] private Sprite _inRangeSprite = default;
        [SerializeField] private Color _inRangeColor = Color.red;

        private Color _defaultColor;
        private Sprite _defaultSprite;
        private MiningTool.MiningTool _miningTool;

        protected void Awake() {
            _defaultColor = _icon.color;
            _defaultSprite = _icon.sprite;
        }

        public void SetMiningTool(MiningTool.MiningTool miningTool) {
            if (_miningTool) _miningTool.InRange.Unsubscribe(OnInRange);
            _miningTool = miningTool;
            if (!_miningTool) return;
            _miningTool.InRange.Subscribe(OnInRange);
        }

        private void OnDestroy() {
            if (_miningTool) _miningTool.InRange.Unsubscribe(OnInRange);
        }

        private void OnInRange(bool inRange) {
            _icon.color = inRange ? _inRangeColor : _defaultColor;
            _icon.sprite = inRange ? _inRangeSprite : _defaultSprite;
        }

        private void Reset() {
            _icon = GetComponent<Image>();
        }
    }
}