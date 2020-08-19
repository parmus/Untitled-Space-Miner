using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SpaceGame.Utility
{
    public class SavingSystem
    {
        private readonly IFormatter  _formatter;

        public SavingSystem()
        {
            _formatter = new BinaryFormatter();
            var ss = new SurrogateSelector();

            _formatter.SurrogateSelector = ss;
        }

        public void Save(string saveFile)
        {
            var state = LoadFile(saveFile);
            PersistableEntity.CaptureStates(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            PersistableEntity.RestoreStates(LoadFile(saveFile));
        }

        public static void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
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

        private void SaveFile(string saveFile, object state)
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