#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GamingXRCore.SceneLoader
{
    internal static class MenuItem_UpdateSceneShortcut
    {
        private static readonly char aspas = '"';

        public static void UpdateScenesShortcut(List<string> scenePaths)
        {
            var basePath = SceneLoaderUtility.GetBasePath();

            var templatePath = $"{basePath}/Editor/SceneLoaderTemplate.txt";
            var loadedContent = AssetDatabase.LoadAssetAtPath<TextAsset>(templatePath);
            string sceneItems = string.Empty;

            for (int i = 0; i < scenePaths.Count; i++)
            {
                string sp = scenePaths[i];
                sceneItems += $"[MenuItem({aspas}GamingXRCore/SceneLoader/LoadScene/{Path.GetFileNameWithoutExtension(sp)}{aspas}, priority = {i})]";
                sceneItems += $"\n\tprivate static void Load{Path.GetFileNameWithoutExtension(sp)}() => LoadScene({aspas}{sp}{aspas});\n\n\t";
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