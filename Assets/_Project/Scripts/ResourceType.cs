using UnityEngine;

namespace SpaceGame
{
    [CreateAssetMenu(fileName = "New Resource Type", menuName = "Game Data/Resource Type", order = 1)]
    public class ResourceType : ItemType
    {
        public override string Name => name;
    }
}
