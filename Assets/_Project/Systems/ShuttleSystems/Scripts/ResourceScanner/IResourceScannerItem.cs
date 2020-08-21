using System;
using SpaceGame.Core;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ResourceScanner
{
    public interface IResourceScannerItem {
        ResourceDeposit ResourceDeposit { get; }
        Transform Transform { get; }
        float Distance { get; }
        bool InRange { get; }
        event Action OnDestroy;
    }
}