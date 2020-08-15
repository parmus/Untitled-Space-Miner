using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceGame.Utility.UI
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _showDelay = 0.3f;
        [SerializeField] private GameObject _gameObject = default;
        [SerializeField] private TMPro.TextMeshProUGUI _label = default;
    
        private Coroutine _coroutine;
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.dragging) return;
            if (!_label.enabled || _label.text == "") return;
            _coroutine = StartCoroutine(CO_HoverDelay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _gameObject.SetActive(false);
        }

        private IEnumerator CO_HoverDelay()
        {
            yield return new WaitForSeconds(_showDelay);
            _gameObject.SetActive(true);
        }

        private void Awake() => _gameObject.SetActive(false);
    }
}
