using UnityEditor;

namespace SpaceGame.AsteroidSystem.Editor
{
    [CustomEditor(typeof(ResourceDeposit))]
    public class ResourceDepositEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var resourceDeposit = target as ResourceDeposit;
            if (resourceDeposit != null && resourceDeposit.Type != null)
            {
                EditorGUILayout.LabelField("Max Health", $"{resourceDeposit.MaxHealth}");
            }
        }
    }
}