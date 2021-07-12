using SpaceGame.Utility.GameEvents;
using UnityEngine;

namespace SpaceGame.LevelManagement
{
    [CreateAssetMenu(fileName = "Location Event", menuName = "Game Events/Location Event")]
    public class LocationEvent: GameEvent<Location> {}
}
