using UnityEngine;

namespace SpaceGame.Interactables
{
    public class InteractablePrompt : MonoBehaviour
    {
        [SerializeField] private Selector _selector = default;
        [SerializeField] private GameObject _prompt = default;
        [SerializeField] private TMPro.TMP_Text _promptLabel = default;

        private void Start() => _selector.CurrentInteractable.Subscribe(OnChange);

        private void OnDestroy() => _selector.CurrentInteractable.Unsubscribe(OnChange);

        private void OnChange(IInteractable interactable)
        {
            _prompt.SetActive(interactable != null);
            if (interactable != null) _promptLabel.text = interactable.Prompt;
        }
    }
}