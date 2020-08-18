using System.Collections;
using SpaceGame.InventorySystem;
using SpaceGame.Utility;
using UnityEngine;

namespace SpaceGame.CraftingSystem {
    public class Crafter: MonoBehaviour
    {
        public IReadonlyObservable<Recipe> CurrentlyCrafting => _currentlyCrafting;
        public IReadonlyObservable<CrafterState> State => _state;
        public IReadonlyObservable<float> Progress => _progress;

        private readonly Observable<Recipe> _currentlyCrafting = new Observable<Recipe>();
        private readonly Observable<CrafterState> _state = new Observable<CrafterState>(CrafterState.Idle);
        private readonly Observable<float> _progress = new Observable<float>(0f);

        public IInventory Inventory { get; set; }

        public void Craft(Recipe recipe) {
            if (Inventory == null) return;  // No inventory
            if (!recipe.TakeIngredients(Inventory)) return;  // Not enough ingredients
            if (!Inventory.CanAdd(recipe.Output.Type, recipe.Output.Amount)) return; // No room for output
            if ( _state != CrafterState.Idle) return;  // Busy
            StartCoroutine(CO_Craft(recipe));
        }

        private IEnumerator CO_Craft(Recipe recipe) {
            _currentlyCrafting.Value = recipe;
            _state.Value = CrafterState.Crafting;
            _progress.Value = 0f;

            do {
                yield return null;
                _progress.Value = Mathf.Clamp01(_progress.Value + Time.deltaTime / recipe.CraftTime);
            } while (_progress.Value < 1f);

            Inventory.Add(recipe.Output.Type, recipe.Output.Amount);
            _state.Value = CrafterState.Idle;
            _progress.Value = 0f;
            _currentlyCrafting.Value = null;
        }

        public enum CrafterState {
            Idle,
            Crafting
        }
    }
}