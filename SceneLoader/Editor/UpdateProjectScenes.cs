#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GamingXRCore.SceneLoader
{
    internal static class UpdateProjectScenes
    {
        public static void GetAllScenes(List<string> scenes)
        {
            var basePath = SceneLoaderUtility.GetBasePath();

            CreateEnum(scenes, "ProjectScenes", basePath);

            AssetDatabase.Refresh();

            //Ping Object
            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(Path.Combine($"{basePath}/Runtime"));
            EditorGUIUtility.PingObject(obj);
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