using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceGame.Utility.UI
{
    [RequireComponent(typeof(ITooltipProvider))]
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _showDelay = 0.3f;
   
        private Coroutine _coroutine;
        private ITooltipProvider _tooltipProvider;

        private void Awake() => _tooltipProvider = GetComponent<ITooltipProvider>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.dragging) return;
            _coroutine = StartCoroutine(CO_HoverDelay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            TooltipUI.Hide();
        }

        private IEnumerator CO_HoverDelay()
        {
            var tooltipContent = _tooltipProvider.GetTooltip();
            if (string.IsNullOrEmpty(tooltipContent)) yield break;

            yield return new WaitForSeconds(_showDelay);
            TooltipUI.SetText(tooltipContent);
        }
    }
}
