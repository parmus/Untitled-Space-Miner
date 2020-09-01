using UnityEngine;

namespace SpaceGame.Utility.UI
{
    public class StaticTooltipProvider : MonoBehaviour, ITooltipProvider
    {
        [SerializeField][TextArea(3,10)] private string _tooltip = default;

        public string GetTooltip() => _tooltip;
    }
}