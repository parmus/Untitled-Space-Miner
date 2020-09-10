using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.Interactables
{
    public class InteractableButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent _onInteract = default;
        [SerializeField] private string _prompt = default;

        public void Interact() => _onInteract.Invoke();
        public string Prompt => _prompt;
    }
}