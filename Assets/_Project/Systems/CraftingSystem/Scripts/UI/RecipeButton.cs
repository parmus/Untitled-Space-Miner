using SpaceGame.InventorySystem;
using SpaceGame.Utility.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceGame.CraftingSystem.UI
{
    [SelectionBase]
    public class RecipeButton : MonoBehaviour, ITooltipProvider
    {
        [SerializeField] private Crafter _crafter = default;
        [SerializeField] private Recipe _recipe = default;

        [SerializeField] private Button _button = default;
        [SerializeField] private Image _thumbnail = default;
        [SerializeField] private UnityEvent<float> _onProgress = default;

        private IInventory _inventory;
        
        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            _crafter.CurrentlyCrafting.Subscribe(OnCurrentlyCrafting);
            _crafter.Progress.Subscribe(OnProgress);
            _crafter.Inventory.Subscribe(OnInventoryChange);
            UpdateInteractable();
        }

        private void OnInventoryChange(IInventory inventory)
        {
            if (_inventory != null) _inventory.OnChange -= UpdateInteractable;
            _inventory = inventory;
            if (_inventory == null) return;
            _inventory.OnChange += UpdateInteractable;
            UpdateInteractable();
        }

        private void UpdateInteractable() =>
            _button.interactable = _inventory != null && _recipe.HasIngredients(_inventory) &&
                                   _crafter.CurrentlyCrafting.Value == null;

        private void OnProgress(float progress) =>
            _onProgress.Invoke(_crafter.CurrentlyCrafting.Value == _recipe ? progress : 0);

        private void OnDestroy()
        {
            if (_crafter == null) return;
            _crafter.CurrentlyCrafting.Unsubscribe(OnCurrentlyCrafting);
            _crafter.Progress.Unsubscribe(OnProgress);
        }

        private void OnCurrentlyCrafting(Recipe recipe) => UpdateInteractable();

        
        private void OnClick() => _crafter.Craft(_recipe);

        private void OnValidate()
        {
            if (_recipe == null) return;
            if (_thumbnail != null) _thumbnail.sprite = _recipe.Thumbnail;
            gameObject.name = $"Craft {_recipe.Output.Type.Name} Button";
        }

        public string GetTooltip() => _recipe == null ? null : _recipe.GetDescription();
    }
}