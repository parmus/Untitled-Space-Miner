using DG.Tweening;
using SpaceGame.Core;
using SpaceGame.Utility.UI;
using UnityEngine;

namespace SpaceGame.InventorySystem.UI {
    [SelectionBase]
    public class InventorySlot : MonoBehaviour, IStackProvider, ITooltipProvider
    {
        [SerializeField] private UnityEngine.UI.Image _frame = default;
        [SerializeField] private TMPro.TextMeshProUGUI _amountLabel = default;
        [SerializeField] private UnityEngine.UI.Image _thumbnail = default;
        [SerializeField] private ProgressBar _stackFilledProgress = default;
        
        [Header("Tween settings")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Color _flashColor = Color.yellow;
        [SerializeField] private Ease _flashEase = Ease.InQuad;
        [SerializeField] private float _scale = 1.2f;
        [SerializeField] private Ease _scaleEase = Ease.InQuad;

        private Vector3 _scaleVector; 
        private Sequence _seq;

        private IInventoryStack _stack = null;
        private ItemType _prevType;
        private uint _prevAmount;
        
        public IInventoryStack Stack {
            get => _stack;
            set
            {
                if (_stack == value) return;
                _stack = value;
                _prevType = _stack.Type;
                _prevAmount = _stack.Amount;
                UpdateUI();
            }
        }

        public void OnChange()
        {
            if (_prevType == _stack.Type && _prevAmount == _stack.Amount) return;
            _prevType = _stack.Type;
            _prevAmount = _stack.Amount;
            UpdateUIWithAnimation();
        }

        private void UpdateUIWithAnimation() {
            _seq?.Complete(true);
            _seq = DOTween.Sequence();
            _seq.Insert(0, _frame.DOColor(_flashColor, _duration / 2).SetLoops(2, LoopType.Yoyo).SetEase(_flashEase));
            _seq.Insert(0, _frame.rectTransform.DOScale(_scaleVector, _duration / 2).SetLoops(2, LoopType.Yoyo).SetEase(_scaleEase));
            _seq.InsertCallback(_duration / 2, UpdateUI);
        }

        private void UpdateUI() {
            if (_stack?.Type == null) {
                _amountLabel.enabled = false;
                _thumbnail.enabled = false;
                _stackFilledProgress.gameObject.SetActive(false);
            } else {
                _thumbnail.enabled = true;
                _amountLabel.enabled = _stack.Type.CanStack;
                _stackFilledProgress.gameObject.SetActive(_stack.Type.CanStack);

                _thumbnail.sprite = _stack.Type.Thumbnail;

                if (!_stack.Type.CanStack) return;
                _amountLabel.text = _stack.Amount.ToString();
                _stackFilledProgress.Value = _stack.PercentageFull;
            }
        }

        private void Awake()
        {
            UpdateUI();
            _scaleVector = new Vector3(_scale, _scale, _scale);
        }

        public string GetTooltip() => _stack?.Type == null ? null : _stack.Type.Tooltip;
    }
}
