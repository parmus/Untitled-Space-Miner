using SpaceGame.Core;
using UnityEngine;

namespace SpaceGame.ShuttleSystems
{
    public abstract class ShuttleUpgrade : ItemType
    {
        [SerializeField] private string _description = default;

        public override string Description => _description;
        public override string Tooltip => $"<b></u>{Name}</u><b>\n{Description}";
    }
}