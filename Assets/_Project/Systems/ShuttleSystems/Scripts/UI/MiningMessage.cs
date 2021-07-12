using SpaceGame.Core;
using UnityEngine;
using DG.Tweening;

namespace SpaceGame.ShuttleSystems.UI
{
    public class MiningMessage : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeIn = 0.3f;
        [SerializeField] private float _delay = 2.5f;
        [SerializeField] private float _fadeOut = 1.0f;

        private MiningTool.MiningTool _miningTool;
        private Sequence _seq;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _seq = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1, _fadeIn))
                .AppendInterval(_delay)
                .Append(_canvasGroup.DOFade(0f, _fadeOut))
                .SetAutoKill(false)
                .Pause();
        }

        private void OnMessage(ItemType itemType, uint amount)
        {
            _seq.Restart();
            _text.text = amount == 0 ?  "Shuttle inventory full!" : $"+{amount} {itemType.Name}";
        }

        public void SetMiningTool(MiningTool.MiningTool miningTool)
        {
            if (_miningTool != null) _miningTool.OnResourceAcquired -= OnMessage;
            _miningTool = miningTool;
            if (_miningTool != null) _miningTool.OnResourceAcquired += OnMessage;
        }
    }
}
