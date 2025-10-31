#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GamingXRCore.SceneLoader
{
    static class MenuItem_GetProjectScenes
    {
        [MenuItem("GamingXRCore/SceneLoader/Update Project Scenes", priority = 3)]
        private static void GetAllScenes()
        {
            var basePath = MenuItem_UpdateSceneShortcut.GetBasePath();

            //Pega todas as cenas que estão na build
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            List<string> scenes = new List<string>();

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                scenes.Add(sceneName);
            }

            CreateEnum(scenes, "ProjectScenes", basePath);

            AssetDatabase.Refresh();

            //Ping Object
            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(Path.Combine($"{basePath}/Runtime"));
            EditorGUIUtility.PingObject(obj);

            MenuItem_UpdateSceneShortcut.UpdateScenesShortcut();
        }

        private static void CreateEnum(List<string> scenes, string enumName, string basePath)
        {
            string content = $"namespace GamingXRCore.SceneLoader\r\n{{" +
                $"/*This enum is auto generate by Editor/MenuItem_GetProjectScenes*/" +
                $"\npublic enum {enumName}" +
                $"\n{{\n\t{string.Join(",\n\t", scenes.Select(p => p))}\n}}\n}}";

            //Caso o caminho não exista
            string path = Path.Combine($"{basePath}/Runtime");

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            File.WriteAllText($"{path}/{enumName}.generated.cs", content);
        }
    }
}
#endif