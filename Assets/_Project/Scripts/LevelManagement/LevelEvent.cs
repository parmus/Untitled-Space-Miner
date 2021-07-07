using SpaceGame.Utility.GameEvents;
using UnityEngine;

namespace SpaceGame.LevelManagement
{
    [CreateAssetMenu(fileName = "Level Event", menuName = "Game Events/Level Event")]
    public class LevelEvent: GameEvent<Level> {}
}
