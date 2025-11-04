#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace GamingXRCore.SceneLoader
{
    public static class SceneLoaderUtility
    {
        internal static string GetBasePath()
        {
            var className = "SceneLoaderUtility";
            string basePath = string.Empty;

            string[] guids = AssetDatabase.FindAssets($"t:Script {className}");
            if (guids.Length > 0)
            {
                basePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                basePath = basePath.Replace($"/Editor/{className}.cs", string.Empty);
            }

            return basePath;
        }

        public static List<string> GetScenesInBuild()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            List<string> scenes = new List<string>();

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                scenes.Add(sceneName);
            }

            return scenes;
        }
    }
}
#endif