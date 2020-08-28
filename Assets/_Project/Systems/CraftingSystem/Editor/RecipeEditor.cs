using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.CraftingSystem.Editor
{
    [CustomEditor(typeof(Recipe))]
    public class RecipeEditor : UnityEditor.Editor
    {
        private static float _margin = 5f;
        
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
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      
        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            var element = _ingredientsProp.GetArrayElementAtIndex(index);
            var amountProp = element.FindPropertyRelative("_amount");
            var typeProp = element.FindPropertyRelative("_type");

            EditorGUI.PropertyField(new Rect(
                rect.x,
                rect.y,
                50 - _margin,
                rect.height - EditorGUIUtility.standardVerticalSpacing
            ), amountProp, GUIContent.none);

            EditorGUI.PropertyField(new Rect(
                rect.x + 50,
                rect.y,
                rect.width - 50,
                rect.height - EditorGUIUtility.standardVerticalSpacing
            ), typeProp, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Output");
            EditorGUILayout.BeginHorizontal("Box");
            {
                var amountProp = _outputProp.FindPropertyRelative("_amount");
                var typeProp = _outputProp.FindPropertyRelative("_type");
                
                EditorGUILayout.PropertyField(amountProp, GUIContent.none, GUILayout.Width(50 - _margin));
                EditorGUILayout.PropertyField(typeProp, GUIContent.none);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_craftTimeProp);
            EditorGUILayout.Space();
            _ingredientsList.DoLayoutList();

            if (!serializedObject.hasModifiedProperties) return;

            serializedObject.ApplyModifiedProperties();
        }
    }
}