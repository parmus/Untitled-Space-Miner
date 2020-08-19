using UnityEngine;

namespace SpaceGame.Core
{
    [CreateAssetMenu(fileName = "New Resource Type", menuName = "Game Data/Resource Type", order = 1)]
    public class ResourceType : ItemType
    {
        [SerializeField] private float _hardness = 100f;
        
        public override string Name => name;
        public float Hardness => _hardness;
    }
}
