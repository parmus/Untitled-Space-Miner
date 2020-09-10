using UnityEngine;

namespace SpaceGame.Interactables
{
    public class InteractableTester : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt = "Do the thing!";
        
        public void Interact() => Debug.Log($"Interacting with {gameObject.name}");
        public string Prompt => _prompt;
    }
}