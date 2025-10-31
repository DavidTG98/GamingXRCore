#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace GamingXRCore.SceneLoader
{
    public static class MenuItem_UpdateSceneShortcut
    {
        private static readonly char aspas = '"';

        [MenuItem("Window/GetClassName")]
        public static void GetClassName()
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame frame = stackTrace.GetFrame(0);
            string fullPath = frame.GetFileName();
        }

        public static string GetBasePath()
        {
            var className = "MenuItem_UpdateSceneShortcut";
            string basePath = string.Empty;

            string[] guids = AssetDatabase.FindAssets($"t:Script {className}");
            if (guids.Length > 0)
            {
                basePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                basePath = basePath.Replace($"/Editor/{className}.cs", string.Empty);
            }

            return basePath;
        }

        public static void UpdateScenesShortcut()
        {
            var basePath = GetBasePath();

            var templatePath = $"{basePath}/Editor/SceneLoaderTemplate.txt";
            var loadedContent = AssetDatabase.LoadAssetAtPath<TextAsset>(templatePath);
            string sceneItems = string.Empty;

            //Pega todas as cenas que estão na build
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