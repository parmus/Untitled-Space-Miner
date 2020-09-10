namespace SpaceGame.Interactables
{
    public interface IInteractable
    {
        void Interact();
        string Prompt { get; }
    }
}