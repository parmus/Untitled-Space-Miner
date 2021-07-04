using System;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
#endif




namespace SpaceGame.LevelManagement
{
    [Serializable]
    public class SceneAsset
    {
#if UNITY_EDITOR
        [ValueDropdown("@SceneAsset.ScenesInBuildConfiguration()", DropdownTitle = "Select Scene")]
        [HideLabel]
        [Required]
#endif
        public string ScenePath;

        public int BuildIndex => SceneUtility.GetBuildIndexByScenePath(ScenePath);
        public Scene Scene => SceneManager.GetSceneByPath(ScenePath);
        
#if UNITY_EDITOR
        public static IEnumerable ScenesInBuildConfiguration()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:SceneAsset")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath((string) x))
                .Where(x => SceneUtility.GetBuildIndexByScenePath(x) > -1);
        }
#endif
    }
}
