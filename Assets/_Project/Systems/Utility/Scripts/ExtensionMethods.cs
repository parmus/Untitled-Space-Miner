using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Utility
{
    public static class ExtensionMethods {
        public static T PickRandom<T>(this IReadOnlyList<T> objects) {
            return objects[Random.Range(0, objects.Count)];
        }

        public static void DestroyAllChildren(this Transform transform) {
            foreach(Transform child in transform) {
                Object.Destroy(child.gameObject);
            }
        }
        
        public static void Swap<T>(ref T a, ref T b) {
            var tmp = a;
            a = b;
            b = tmp;
        }
    }
}