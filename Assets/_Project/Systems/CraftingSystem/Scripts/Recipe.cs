using System;
using System.Collections.Generic;
using System.Text;
using SpaceGame.Core;
using SpaceGame.InventorySystem;
using UnityEngine;

namespace SpaceGame.CraftingSystem {
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Game Data/Crafting Recipe", order = 1)]
    public class Recipe : ScriptableObject {
        #region Serialized fields
        [SerializeField] private List<Ingredient> _ingredients = new List<Ingredient>();
        [SerializeField] private Ingredient _output = new Ingredient();
        [SerializeField] private float _craftTime = 1f;
        #endregion

        #region Public properties
        public Sprite Thumbnail => _output.Type ? _output.Type.Thumbnail : null;
        public IReadOnlyList<Ingredient> Ingredients => _ingredients;
        public Ingredient Output => _output;
        public float CraftTime => _craftTime;
        #endregion

        #region Public methods
        public bool HasIngredients(IInventory inventory) {
            foreach(var ingredient in _ingredients) {
                if (!inventory.Has(ingredient.Type, ingredient.Amount)) return false;
            }
            return true;
        }

        public string GetDescription()
        {
            var sb = new StringBuilder();
            sb.Append($"<b><u>{_output.Type.Name}");
            if (_output.Amount > 1) sb.Append($" (x{_output.Amount})</u></b>");
            sb.AppendLine("</u></b>");
            sb.AppendLine($"{_output.Type.Description}");
            sb.AppendLine("Ingredients:");
            sb.Append("<#ff0000>");
            _ingredients.ForEach(ingredient =>
                sb.AppendLine(ingredient.Amount > 1 ?
                    $"• {ingredient.Type.Name} x{ingredient.Amount}" :
                    $"• {ingredient.Type.Name}")
            );
            sb.Append("</color>");

            return sb.ToString();
        }

        public bool TakeIngredients(IInventory inventory) {
            if (!HasIngredients(inventory)) return false;
            foreach(var ingredient in _ingredients) {
                inventory.Remove(ingredient.Type, ingredient.Amount);
            }
            return true;
        }
        #endregion

        #region Ingredient class
        [Serializable]
        public class Ingredient {
            [SerializeField] private ItemType _type;
            [SerializeField] private uint _amount = 1;

            public ItemType Type => _type;
            public uint Amount => _amount;
        }
        #endregion
    }
}
