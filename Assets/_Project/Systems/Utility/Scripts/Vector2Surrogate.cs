using System.Runtime.Serialization;
using UnityEngine;

namespace SpaceGame.Utility
{
    public class Vector2Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var vector2 = (Vector2) obj;
            info.AddValue("x", (double) vector2.x);
            info.AddValue("y", (double) vector2.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var x = (float) info.GetDouble("x");
            var y = (float) info.GetDouble("y");
            return new Vector2(x, y);
        }
    }
}