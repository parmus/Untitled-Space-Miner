using SpaceGame.Utility.UI;
using UnityEngine;

namespace SpaceGame.CraftingSystem.Demo.Scripts {
    public class CraftButton : MonoBehaviour
    {
        [SerializeField] private Crafter _crafter = default;
        [SerializeField] private Recipe _recipe = default;


        private UnityEngine.UI.Button _button;
        private ProgressBar _progressBar;

        private void Awake() {
            _button = GetComponent<UnityEngine.UI.Button>();
            _button.onClick.AddListener(() => _crafter?.Craft(_recipe));
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        private void OnEnable() {
            _crafter.Progress.OnChange += OnProgress;
            _crafter.CurrentlyCrafting.OnChange += OnCurrentlyCrafting;
        }

        private void OnDisable() {
            _crafter.Progress.OnChange -= OnProgress;
            _crafter.CurrentlyCrafting.OnChange -= OnCurrentlyCrafting;
        }

        private void OnProgress(float progress) => _progressBar.Value = progress;

        private void OnCurrentlyCrafting(Recipe recipe) {
            _button.interactable = recipe == null;
            _progressBar.gameObject.SetActive(recipe == _recipe);
        }

        private void OnValidate() {
            if (_recipe == null) return;
            var label = GetComponentInChildren<UnityEngine.UI.Text>();
            if (label == null) return;
            label.text = "Craft " + _recipe.Output.Type.Name;
            gameObject.name = "Craft " + _recipe.Output.Type.Name + " button";
        }
    }
}