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
            [SerializeField] private ItemType _type;
            [SerializeField] private uint _amount = 1;

            public ItemType Type => _type;
            public uint Amount => _amount;
        }

        private void Reset()
        {
            var go = gameObject;
            go.name = GetType().Name;
            go.tag = "EditorOnly";
        }
        
        [MenuItem("GameObject/Session Inventory Injector", false, 10)]
        private static void CreateSessionInventoryInjector(MenuCommand menuCommand)
        {
            var go = new GameObject();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<SessionInventoryInjector>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}    
#endif
