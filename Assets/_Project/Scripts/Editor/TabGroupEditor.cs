using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.Editor
{
    [CustomEditor(typeof(TabGroup))]
    public class TabGroupEditor : UnityEditor.Editor
    {
        private SerializedObject _so;
        private SerializedProperty _currentTabProperty;
        private SerializedProperty _tabButtonsProperty;
        private SerializedProperty _tabsProperty;
        private SerializedProperty _normalTintProperty;
        private SerializedProperty _selectedTintProperty;
        private SerializedProperty _hoverTintProperty;

        private void OnEnable()
        {
            _so = serializedObject;
            _currentTabProperty = _so.FindProperty("_currentTab");
            _tabButtonsProperty = _so.FindProperty("_tabButtons");
            _tabsProperty = _so.FindProperty("_tabs");
            _normalTintProperty = _so.FindProperty("_normalTint");
            _selectedTintProperty = _so.FindProperty("_selectedTint");
            _hoverTintProperty = _so.FindProperty("_hoverTint");
            Assert.IsNotNull(_currentTabProperty);
            Assert.IsNotNull(_tabButtonsProperty);
            Assert.IsNotNull(_tabsProperty);
            Assert.IsNotNull(_normalTintProperty);
            Assert.IsNotNull(_selectedTintProperty);
            Assert.IsNotNull(_hoverTintProperty);
        }

        public override void OnInspectorGUI()
        {
            _so.Update();
            EditorGUILayout.PropertyField(_currentTabProperty);
            EditorGUILayout.PropertyField(_tabButtonsProperty);
            EditorGUILayout.PropertyField(_tabsProperty);
            if (_tabsProperty.arraySize != _tabButtonsProperty.arraySize)
            {
                EditorGUILayout.HelpBox("Button and tab amount mismatch", MessageType.Error);
            }

            EditorGUILayout.PropertyField(_normalTintProperty);
            EditorGUILayout.PropertyField(_selectedTintProperty);
            EditorGUILayout.PropertyField(_hoverTintProperty);
            
            if (!_so.hasModifiedProperties) return;
            
            _so.ApplyModifiedProperties();
            
            _so.Update();
            _currentTabProperty.intValue = Mathf.Clamp(_currentTabProperty.intValue, 0, _tabsProperty.arraySize - 1);
            _so.ApplyModifiedProperties();

            for (var i = 0; i < _tabsProperty.arraySize; i++)
            {
                var tab = _tabsProperty.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;
                if (tab != null)
                {
                    tab.SetActive(_currentTabProperty.intValue == i);
                }
            }

        }
    }
}