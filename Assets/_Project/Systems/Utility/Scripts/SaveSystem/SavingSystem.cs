using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SpaceGame.Utility.SaveSystem
{
    public static class SavingSystem
    {
        private static readonly IFormatter  _formatter;
        private static readonly SurrogateSelector _ss;

        static SavingSystem () {
            _ss = new SurrogateSelector();
            
            _ss.AddSurrogate(
                typeof(Vector2),
                new StreamingContext(StreamingContextStates.All),
                new Vector2Surrogate()
            );
            _ss.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3Surrogate()
            );
            _ss.AddSurrogate(
                typeof(Quaternion),
                new StreamingContext(StreamingContextStates.All),
                new QuaternionSurrogate()
            );
            
            _formatter = new BinaryFormatter {SurrogateSelector = _ss};
        }

        public static void Save(string saveFile)
        {
            var state = LoadFile(saveFile);
            PersistableEntity.CaptureStates(state);
            SaveFile(saveFile, state);
        }

        public static void Load(string saveFile)
        {
            PersistableEntity.RestoreStates(LoadFile(saveFile));
        }

        public static void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private static Dictionary<string, object> LoadFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (var stream = File.Open(path, FileMode.Open))
            {
                return (Dictionary<string, object>)_formatter.Deserialize(stream);
            }
        }

        private static void SaveFile(string saveFile, object state)
        {
            var path = GetPathFromSaveFile(saveFile);
            using (var stream = File.Open(path, FileMode.Create))
            {
                _formatter.Serialize(stream, state);
            }
        }

        private static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}