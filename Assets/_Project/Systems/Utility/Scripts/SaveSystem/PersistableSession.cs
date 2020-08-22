using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SpaceGame.Utility.SaveSystem
{
    public class PersistableSession
    {
        public static readonly SurrogateSelector SurrogateSelector;
        public Dictionary<string, object> State {
            get
            {
                if (_state == null) Load();
                return _state;
            }
        }

        
        #region Public methods
        public PersistableSession(string filename) => _filename = filename;

        public void Save() => SaveFile(_filename, _state);

        public void Load() => _state = LoadFile(_filename);
        
        public void Delete() => File.Delete(GetPathFromSaveFile(_filename));
        #endregion

        
        #region Private variables
        private Dictionary<string, object> _state = null;
        private readonly string _filename;
        #endregion
        
        
        #region Private static methods and variables
        private static readonly IFormatter  _formatter;
        
        
        static PersistableSession () {
            SurrogateSelector = new SurrogateSelector();
            
            SurrogateSelector.AddSurrogate(
                typeof(Vector2),
                new StreamingContext(StreamingContextStates.All),
                new Vector2Surrogate()
            );
            SurrogateSelector.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3Surrogate()
            );
            SurrogateSelector.AddSurrogate(
                typeof(Quaternion),
                new StreamingContext(StreamingContextStates.All),
                new QuaternionSurrogate()
            );
            
            _formatter = new BinaryFormatter {SurrogateSelector = SurrogateSelector};
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
        
        public static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
        #endregion
    }
}