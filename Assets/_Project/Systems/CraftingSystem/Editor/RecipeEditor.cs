using SpaceGame.Utility;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.CraftingSystem.Editor
{
    [CustomEditor(typeof(Recipe))]
    public class RecipeEditor : UnityEditor.Editor
    {
        private SerializedProperty _ingredientsProp;
        private SerializedProperty _outputProp;
        private SerializedProperty _craftTimeProp;

        private ReorderableList _ingredientsList;
        
        private void OnEnable()
        {
            _ingredientsProp = serializedObject.FindProperty("_ingredients");
            _outputProp = serializedObject.FindProperty("_output");
            _craftTimeProp = serializedObject.FindProperty("_craftTime");
            
            Assert.IsNotNull(_ingredientsProp);
            Assert.IsNotNull(_outputProp);
            Assert.IsNotNull(_craftTimeProp);
            
            _ingredientsList = new ReorderableList(serializedObject, _ingredientsProp, true, true, true, true)
            {
                drawHeaderCallback = DrawHeaderCallback,
                elementHeightCallback = ElementHeightCallback,
                drawElementCallback = DrawElementCallback
            };
        }

        private void DrawHeaderCallback (Rect rect) => 
            EditorGUI.LabelField(rect, new GUIContent("Ingredients"));


        private float ElementHeightCallback(int index) =>
            EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
      
        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.PropertyField(
                rect.WithMargin(
                    top: EditorGUIUtility.standardVerticalSpacing,
                    bottom:EditorGUIUtility.standardVerticalSpacing
                ),
                _ingredientsProp.GetArrayElementAtIndex(index),
                GUIContent.none
            );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Output");
            EditorGUILayout.PropertyField(_outputProp, GUIContent.none);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_craftTimeProp);
            EditorGUILayout.Space();
            _ingredientsList.DoLayoutList();

            if (!serializedObject.hasModifiedProperties) return;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
