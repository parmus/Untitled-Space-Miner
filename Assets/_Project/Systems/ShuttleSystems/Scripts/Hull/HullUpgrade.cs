using UnityEngine;

namespace SpaceGame.ShuttleSystems.Hull
{
    [CreateAssetMenu(fileName = "New Hull Upgrade", menuName = "Game Data/Hull Upgrade", order = 1)]
    public class HullUpgrade : ShuttleUpgrade, IHullConfiguration
    {
        #region Serialized fields
        [SerializeField] private float _hullStrength = 1f;
        #endregion


        #region Properties
        public override string Name => name;
        public override string Description => $"{base.Description}\n<#00ff00>• Hull Strength: {_hullStrength}</color>";
        public float HullStrength => _hullStrength;
        #endregion
    }
}