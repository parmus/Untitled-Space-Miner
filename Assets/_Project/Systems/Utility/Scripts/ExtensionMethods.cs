using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Utility
{
    public static class ExtensionMethods {

        #region float
        public static bool Approximate(this float value, float compareTo) => Mathf.Approximately(value, compareTo);
        public static bool IsZero(this float value) => Mathf.Approximately(value, 0f);
        #endregion

        #region IReadOnlyList<T>
        public static T PickRandom<T>(this IReadOnlyList<T> objects) {
            return objects[Random.Range(0, objects.Count)];
        }
        #endregion

        #region UnityEngine.Transform
        public static void DestroyAllChildren(this Transform transform) {
            for (var i = transform.childCount-1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);    
            }
        }
        
        public static void DestroyAllChildrenImmediate(this Transform transform) {
            for (var i = transform.childCount-1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);    
            }
        }

        public static void DestroyAllChildren<T>(this Transform transform) where T: MonoBehaviour
        {
            var childrenToDestroy = transform.GetComponentsInChildren<T>(); 
            for (var i = childrenToDestroy.Length-1; i >= 0; i--)
            {
                Object.Destroy(childrenToDestroy[i].gameObject);    
            }
        }

        public static void DestroyAllChildrenImmediate<T>(this Transform transform) where T: MonoBehaviour
        {
            var childrenToDestroy = transform.GetComponentsInChildren<T>(); 
            for (var i = childrenToDestroy.Length-1; i >= 0; i--)
            {
                Object.DestroyImmediate(childrenToDestroy[i].gameObject);    
            }
        }
        #endregion

        #region Any type
        public static void Swap<T>(ref T a, ref T b) {
            var tmp = a;
            a = b;
            b = tmp;
        }
        #endregion
        
    }
}