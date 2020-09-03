using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Utility
{
    public static class ExtensionMethods {

        #region float
        public static bool Approximate(this float value, float compareTo) => Mathf.Approximately(value, compareTo);
        public static bool IsZero(this float value) => Mathf.Approximately(value, 0f);
        #endregion

        #region Rect
        public static Rect Copy(this Rect rect) => new Rect(rect.position, rect.size);

        public static Rect SetWidth(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }

        public static Rect SetMargin(this Rect rect, float top=0, float bottom=0, float left=0, float right=0)
        {
            rect.yMin += top;
            rect.yMax -= bottom;
            rect.xMin += left;
            rect.xMax -= right;
            return rect;
        }

        public static Rect WithWidth(this Rect rect, float width) => rect.Copy().SetWidth(width);

        public static Rect WithMargin(this Rect rect, float top=0, float bottom=0, float left=0, float right=0) =>
            rect.Copy().SetMargin(top, bottom, left, right);
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