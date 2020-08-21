using System.Runtime.Serialization;
using UnityEngine;

namespace SpaceGame.Utility
{
    public class QuaternionSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var quaternion = (Quaternion) obj;
            info.AddValue("x", (double) quaternion.x);
            info.AddValue("y", (double) quaternion.y);
            info.AddValue("z", (double) quaternion.z);
            info.AddValue("w", (double) quaternion.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var x = (float) info.GetDouble("x");
            var y = (float) info.GetDouble("y");
            var z = (float) info.GetDouble("z");
            var w = (float) info.GetDouble("w");
            return new Quaternion(x, y, z, w);
        }
    }
}