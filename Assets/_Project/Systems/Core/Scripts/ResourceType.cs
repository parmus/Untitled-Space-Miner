using UnityEngine;

namespace SpaceGame.Core
{
    [CreateAssetMenu(fileName = "New Resource Type", menuName = "Game Data/Resource Type", order = 1)]
    public class ResourceType : ItemType
    {
        [SerializeField] private string _description = default;
        [SerializeField] private float _hardness = 100f;
        
        public override string Name => name;
        public override string Description => $"{_description}\n<#00ff00>Hardness: {_hardness}</color>";
        public override string Tooltip => $"<b><u>{Name}</u></b>\n\n{Description}";
        public float Hardness => _hardness;
    }
}
