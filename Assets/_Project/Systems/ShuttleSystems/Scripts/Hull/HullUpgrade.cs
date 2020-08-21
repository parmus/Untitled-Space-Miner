using UnityEngine;

namespace SpaceGame.ShuttleSystems.Hull
{
    [CreateAssetMenu(fileName = "New Hull Upgrade", menuName = "Game Data/Hull Upgrade", order = 1)]
    public class HullUpgrade : ShuttleUpgrade<HullUpgrade>, IHullConfiguration
    {
        #region Serialized fields
        [SerializeField] private float _hullStrength = 1f;
        #endregion


        #region Properties
        public override string Name => name;
        public float HullStrength => _hullStrength;
        #endregion
    }
}