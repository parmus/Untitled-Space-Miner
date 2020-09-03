using SpaceGame.Utility;
using UnityEditor;
using UnityEngine;

namespace SpaceGame.CraftingSystem.Editor
{
    [CustomPropertyDrawer(typeof(Recipe.Ingredient))]
    public class RecipeIngredientDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var amountRect = position.WithWidth(50);
            var typeRect = position.WithMargin(left: amountRect.width + 5);
            
            var amountProp = property.FindPropertyRelative("_amount");
            var typeProp = property.FindPropertyRelative("_type");
            
            EditorGUI.PropertyField(amountRect, amountProp, GUIContent.none);
            EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);

            EditorGUI.indentLevel = oldIndentLevel;
            EditorGUI.EndProperty();
        }
    }
}