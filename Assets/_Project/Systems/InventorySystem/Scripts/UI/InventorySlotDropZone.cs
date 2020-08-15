using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceGame.InventorySystem.UI
{
    public class InventorySlotDropZone: MonoBehaviour, IDropHandler
    {
        [SerializeField] private InventorySlot _inventorySlot = default;
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            var stackProvider = eventData.pointerDrag.GetComponent<IStackProvider>();
            if (stackProvider == null) return;

            var stack = _inventorySlot.Stack;
            var otherStack = stackProvider.Stack;
            if (otherStack == stack) return;

            var added = stack.TryAdd(otherStack.Type, otherStack.Amount);
            otherStack.TryRemove(added);
        }

        private void Reset() => _inventorySlot = GetComponent<InventorySlot>();
    }
}