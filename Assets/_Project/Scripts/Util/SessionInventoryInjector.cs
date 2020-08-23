using System.Collections.Generic;
using SpaceGame.Core;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace SpaceGame.Util
{
    public class SessionInventoryInjector : MonoBehaviour
    {
        [SerializeField] private List<Item> _items = new List<Item>();

        private void Start()
        {
            foreach (var item in _items)
            {
                SessionManager.Instance.Inventory.Add(item.Type, item.Amount);
            }
        }

        [System.Serializable]
        public class Item
        {
            [SerializeField] private ItemType _type = default;
            [SerializeField] private uint _amount = 1;

            public ItemType Type => _type;
            public uint Amount => _amount;
        }

        private void Reset()
        {
            gameObject.name = GetType().Name;
            gameObject.tag = "EditorOnly";
        }
        
        [MenuItem("GameObject/Session Inventory Injector", false, 10)]
        private static void CreateSessionInventoryInjector(MenuCommand menuCommand)
        {
            // Create a custom game object
            var go = new GameObject();
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            go.AddComponent<SessionInventoryInjector>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}    
#endif
