using System;
using System.Collections.Generic;
using SpaceGame.InventorySystem;
using UnityEngine;

namespace SpaceGame.CraftingSystem {
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Game Data/Crafting Recipe", order = 1)]
    public class Recipe : ScriptableObject {
        [Serializable]
        public class Ingredient {
            [SerializeField] private ItemType _type = default;
            [SerializeField] private uint _amount = 1;

            public ItemType Type => _type;
            public uint Amount => _amount;
        }

        [SerializeField] private List<Ingredient> _ingredients = new List<Ingredient>();
        [SerializeField] private Ingredient _output = new Ingredient();
        [SerializeField] private float _craftTime = 1f;

        public Sprite Thumbnail => _output.Type ? _output.Type.Thumbnail : null;

        public IReadOnlyList<Ingredient> Ingredients => _ingredients;
        public Ingredient Output => _output;
        public float CraftTime => _craftTime;

        public bool HasIngredients(IInventory inventory) {
            foreach(var ingredient in _ingredients) {
                if (!inventory.Has(ingredient.Type, ingredient.Amount)) return false;
            }
            return true;
        }

        public bool TakeIngredients(IInventory inventory) {
            if (!HasIngredients(inventory)) return false;
            foreach(var ingredient in _ingredients) {
                inventory.Remove(ingredient.Type, ingredient.Amount);
            }
            return true;
        }
    }


}