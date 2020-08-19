using SpaceGame.Core;

namespace SpaceGame.InventorySystem
{
    public interface IInventoryStack {
        ItemType Type { get; }
        uint Amount { get; }
        float PercentageFull { get; }
        uint Free { get; }
        uint StackSize { get; }

        uint TryAdd(ItemType type, uint amount);
        uint TryAdd(uint amount);
        uint TryRemove(uint amount);
        void Clear();
    }
}