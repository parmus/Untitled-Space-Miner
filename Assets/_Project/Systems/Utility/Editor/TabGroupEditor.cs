using SpaceGame.Utility.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace SpaceGame.Utility.Editor
{
    [CustomEditor(typeof(TabGroup))]
    public class TabGroupEditor : UnityEditor.Editor
    {
        private static float _margin = 5f;
        
        private SerializedProperty _firstTabProperty;
        private SerializedProperty _tabsProperty;
        private SerializedProperty _normalTintProperty;
        private SerializedProperty _selectedTintProperty;
        private SerializedProperty _hoverTintProperty;

        private ReorderableList _tabList;

        private Vector2 _buttonDimensions;

        private void OnEnable()
        {
            _firstTabProperty = serializedObject.FindProperty("_firstTab");
            _tabsProperty = serializedObject.FindProperty("_tabs");
            _normalTintProperty = serializedObject.FindProperty("_normalTint");
            _selectedTintProperty = serializedObject.FindProperty("_selectedTint");
            _hoverTintProperty = serializedObject.FindProperty("_hoverTint");
            Assert.IsNotNull(_firstTabProperty);
            Assert.IsNotNull(_tabsProperty);
            Assert.IsNotNull(_normalTintProperty);
            Assert.IsNotNull(_selectedTintProperty);
            Assert.IsNotNull(_hoverTintProperty);

            _tabList = new ReorderableList(serializedObject, _tabsProperty, true, true, true, true)
            {
                elementHeightCallback = ElementHeightCallback,
                drawElementCallback = DrawElementCallback,
                onAddCallback = OnAddCallback,
                onSelectCallback = OnSelectCallback,
                drawHeaderCallback = DrawHeaderCallback
            };
        }

        private void DrawHeaderCallback(Rect rect) =>
            EditorGUI.LabelField(rect, new GUIContent("Tabs"));

        private void OnSelectCallback(ReorderableList list) => SelectTab(list.index);

        private void OnAddCallback(ReorderableList list)
        {
            Undo.IncrementCurrentGroup();
            
            serializedObject.Update();
            
            var nextIdx = _tabsProperty.arraySize;
            _tabsProperty.InsertArrayElementAtIndex(nextIdx);
            
            var newTabProp = _tabsProperty.GetArrayElementAtIndex(nextIdx);

            var buttonProp = newTabProp.FindPropertyRelative("Button");
            var button = buttonProp.objectReferenceValue as TabButton;
            if (button == null)
            {
                buttonProp.objectReferenceValue = null;
            }
            else
            {
                var newButton = Instantiate(button.gameObject, button.transform.parent);
                newButton.transform.SetAsLastSibling();
                buttonProp.objectReferenceValue = newButton;
                Undo.RegisterCreatedObjectUndo(newButton, "New Button");
            }

            var gameObjectProp = newTabProp.FindPropertyRelative("GameObject"); 
            var gameObject = gameObjectProp.objectReferenceValue as GameObject;
            if (gameObject == null)
            {
                gameObjectProp.objectReferenceValue = null;
            } else {
                var newGameObject = Instantiate(gameObject, gameObject.transform.parent);
                newGameObject.SetActive(true);
                newGameObject.transform.SetAsLastSibling();
                gameObjectProp.objectReferenceValue = newGameObject;
                Undo.RegisterCreatedObjectUndo(newGameObject, "New Tab");
            }
            
            SelectTab(nextIdx);
            
            serializedObject.ApplyModifiedProperties();
            Undo.SetCurrentGroupName("create new tab");
        }

        private float ElementHeightCallback(int index) =>
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private void SelectTab(int index)
        {
            for (var i = 0; i < _tabsProperty.arraySize; i++)
            {
                var tabProp = _tabsProperty.GetArrayElementAtIndex(i);
                var tabGameObject = tabProp.FindPropertyRelative("GameObject").objectReferenceValue as GameObject;
                if (tabGameObject != null) tabGameObject.SetActive(i == index);
            }
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            var element = _tabsProperty.GetArrayElementAtIndex(index);
            var width = (rect.width - _buttonDimensions.x - EditorGUIUtility.singleLineHeight) / 2;
            var offset = rect.x;

            if (EditorGUI.Toggle(new Rect(
                    offset,
                    rect.y,
                    EditorGUIUtility.singleLineHeight,
                    EditorGUIUtility.singleLineHeight
                ),
                GUIContent.none,
                index == _firstTabProperty.intValue
            ))
            {
                serializedObject.Update();
                _firstTabProperty.intValue = index;
                serializedObject.ApplyModifiedProperties();
            }
            offset += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(
                new Rect(
                    offset,
                    rect.y,
                    width-_margin,
                    rect.height-EditorGUIUtility.standardVerticalSpacing
                ),
                element.FindPropertyRelative("Button"),
                new GUIContent("", "Set as first tab")
            );
            offset += width;
            
            EditorGUI.PropertyField(
                new Rect(
                    offset,
                    rect.y,
                    width-_margin,
                    rect.height-EditorGUIUtility.standardVerticalSpacing
                ),
                element.FindPropertyRelative("GameObject"),
                GUIContent.none
            );
            offset += width;
            
            if (GUI.Button(new Rect(
                offset,
                rect.y,
                _buttonDimensions.x,
                rect.height - EditorGUIUtility.standardVerticalSpacing
            ), "Show")) SelectTab(index);
        }

        public override void OnInspectorGUI()
        {
            _buttonDimensions = GUI.skin.button.CalcSize(new GUIContent("Show")) + Vector2.up * _margin;

            serializedObject.Update();
            EditorGUILayout.PropertyField(_firstTabProperty);
            _tabList.DoLayoutList();

            EditorGUILayout.PropertyField(_normalTintProperty);
            EditorGUILayout.PropertyField(_selectedTintProperty);
            EditorGUILayout.PropertyField(_hoverTintProperty);
            
            if (!serializedObject.hasModifiedProperties) return;
           
            _firstTabProperty.intValue = Mathf.Clamp(_firstTabProperty.intValue, 0, _tabsProperty.arraySize - 1);
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("GameObject/Grumpy Bear/UI/TabGroup", false, 10)]
        public static void CreateTabGroup(MenuCommand menuCommand)
        {
            Undo.IncrementCurrentGroup();
            
            var tabGroup = new GameObject("Tab Group", typeof(RectTransform), typeof(TabGroup));
            GameObjectUtility.SetParentAndAlign(tabGroup, menuCommand.context as GameObject);
            var tabGroupRectTransform = tabGroup.GetComponent<RectTransform>();
            tabGroupRectTransform.anchorMin = Vector2.zero;
            tabGroupRectTransform.anchorMax = Vector2.one;
            tabGroupRectTransform.pivot = Vector2.one / 2f;
            tabGroupRectTransform.offsetMin = Vector2.zero;
            tabGroupRectTransform.offsetMax = Vector2.zero;
            Undo.RegisterCreatedObjectUndo(tabGroup, "TabGroup");
            
            var buttons = new GameObject("Tab Buttons", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            var buttonsRectTransform = buttons.GetComponent<RectTransform>();
            buttonsRectTransform.SetParent(tabGroup.transform);
            buttonsRectTransform.anchorMin = Vector2.up;
            buttonsRectTransform.anchorMax = Vector2.one;
            buttonsRectTransform.pivot = Vector2.up + Vector2.right * 0.5f;
            buttonsRectTransform.offsetMin = Vector2.down * 120;
            buttonsRectTransform.offsetMax = Vector2.zero;
            var horizontalLayoutGroup = buttons.GetComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.spacing = 20;
            horizontalLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
            horizontalLayoutGroup.childForceExpandHeight = true;
            horizontalLayoutGroup.childForceExpandWidth = false;
            horizontalLayoutGroup.childControlHeight = true;
            horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            Undo.RegisterCreatedObjectUndo(tabGroup, "Buttons");

            var tabButton = new GameObject("Tab Button", typeof(RectTransform), typeof(TabButton));
            var tabButtonRectTransform = tabButton.GetComponent<RectTransform>();
            tabButtonRectTransform.SetParent(buttons.transform);
            tabButtonRectTransform.pivot = Vector2.up * 0.5f;
            tabButtonRectTransform.offsetMin = Vector2.zero;
            tabButtonRectTransform.offsetMax = Vector2.right * 400;
            Undo.RegisterCreatedObjectUndo(tabGroup, "Tab Button");
            
            var content = new GameObject("Content", typeof(RectTransform));
            var contentRectTransform = content.GetComponent<RectTransform>();
            contentRectTransform.SetParent(tabGroup.transform);
            contentRectTransform.anchorMin = Vector2.zero;
            contentRectTransform.anchorMax = Vector2.one;
            contentRectTransform.pivot = Vector2.one / 2f;
            contentRectTransform.offsetMin = Vector2.zero;
            contentRectTransform.offsetMax = Vector2.down * 120;
            Undo.RegisterCreatedObjectUndo(tabGroup, "Content");
            
            var tab = new GameObject("Tab", typeof(RectTransform));
            var tabRectTransform = tab.GetComponent<RectTransform>();
            tabRectTransform.SetParent(content.transform);
            tabRectTransform.anchorMin = Vector2.zero;
            tabRectTransform.anchorMax = Vector2.one;
            tabRectTransform.pivot = Vector2.one / 2f;
            tabRectTransform.offsetMin = Vector2.zero;
            tabRectTransform.offsetMax = Vector2.zero;
            Undo.RegisterCreatedObjectUndo(tabGroup, "Tab");

            var tabGroupSerializedObject = new SerializedObject(tabGroup.GetComponent<TabGroup>());
            var tabsProperty = tabGroupSerializedObject.FindProperty("_tabs");

            tabGroupSerializedObject.Update();
            var nextIdx = tabsProperty.arraySize;
            tabsProperty.InsertArrayElementAtIndex(nextIdx);
            var newTabProp =tabsProperty.GetArrayElementAtIndex(nextIdx);
            var buttonProp = newTabProp.FindPropertyRelative("Button");
            buttonProp.objectReferenceValue = tabButton.GetComponent<TabButton>();
            var gameObjectProp = newTabProp.FindPropertyRelative("GameObject");
            gameObjectProp.objectReferenceValue = tab;
            tabGroupSerializedObject.ApplyModifiedProperties();
            
            Undo.SetCurrentGroupName("create tab group");
            
            Selection.activeObject = tabGroup;
        }
    }
}