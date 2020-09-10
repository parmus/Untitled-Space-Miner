using UnityEngine;

namespace SpaceGame
{
    public interface ICharacterControls
    {
        bool InRunning { get; }
        Vector2 Movement { get; }
        Vector2 OnLook { get; }
    }
}