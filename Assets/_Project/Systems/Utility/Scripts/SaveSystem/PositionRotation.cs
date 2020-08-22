using UnityEngine;

namespace SpaceGame.Utility.SaveSystem
{
    [System.Serializable]
    public class PositionRotation
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public Vector3 Up => Rotation * Vector3.up;

        public static PositionRotation FromTransform(Transform transform) => new PositionRotation(transform.position, transform.rotation);

        public PositionRotation(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
        
        public void SetTransform(Transform transform)
        {
            transform.position = Position;
            transform.rotation = Rotation;
        }
    }
}