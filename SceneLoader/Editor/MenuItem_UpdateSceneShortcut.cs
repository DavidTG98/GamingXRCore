#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamingXRCore.SceneLoader
{
    internal static class MenuItem_UpdateSceneShortcut
    {
        private static readonly char aspas = '"';

        public static void UpdateScenesShortcut()
        {
            var basePath = SceneLoaderUtility.GetBasePath();

            var templatePath = $"{basePath}/Editor/SceneLoaderTemplate.txt";
            var loadedContent = AssetDatabase.LoadAssetAtPath<TextAsset>(templatePath);
            string sceneItems = string.Empty;

            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                var s = SceneUtility.GetScenePathByBuildIndex(i);

                sceneItems += $"[MenuItem({aspas}GamingXRCore/SceneLoader/LoadScene/{Path.GetFileNameWithoutExtension(s)}{aspas}, priority = {i})]";
                sceneItems += $"\n\tprivate static void Load{Path.GetFileNameWithoutExtension(s)}() => LoadScene({aspas}{s}{aspas});\n\n\t";
            }

            string fullContent = loadedContent.text.Replace("{0}", sceneItems);

            //Caso o caminho não exista
            string path = Path.Combine($"{basePath}/Editor");

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, "MenuItem_SceneShortcut.generated.cs"), fullContent);

            AssetDatabase.Refresh();

            //Ping Object
            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(obj);
        }
    }
}
#endif