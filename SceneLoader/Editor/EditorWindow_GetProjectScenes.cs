using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace GamingXRCore.SceneLoader
{
    internal class EditorWindow_GetProjectScenes : EditorWindow
    {
        private class SceneData
        {
            public string name;
            public string path;
            public bool isSelected;
        }

        private static readonly List<SceneData> scenes = new List<SceneData>();
        private static Vector2 scrollPos;
        private static string inputText;
        private static EditorWindow_GetProjectScenes window;

        // Window settings
        private const float MIN_WIDTH = 450f;
        private const float MIN_HEIGHT = 500f;

        [MenuItem("GamingXRCore/SceneLoader/ProjectScenesWindow")]
        public static void ShowWindow()
        {
            window = GetWindow<EditorWindow_GetProjectScenes>("ProjectScenesWindow");
            window.minSize = new Vector2(MIN_WIDTH, MIN_HEIGHT);
            window.Show();

            GetScenes();
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawAboutTab();
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Get Project Scenes", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Select the scenes from the project to update the GamingXRCore/SceneLoader shortcuts and ProjectScenes enum.", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        private void DrawAboutTab()
        {
            inputText = EditorGUILayout.TextField(inputText);

            var rect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var data in scenes)
            {
                if (!string.IsNullOrWhiteSpace(inputText))
                {
                    if (!data.path.Contains(inputText) && !data.name.Contains(inputText))
                    {
                        continue;
                    }
                }

                data.isSelected = GUILayout.Toggle(data.isSelected, data.name);
                EditorGUILayout.LabelField(data.path, EditorStyles.miniLabel);
                GUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            var bottomButtonStyle = new GUIStyle(GUI.skin.button);
            bottomButtonStyle.fontSize = 16;
            bottomButtonStyle.fontStyle = FontStyle.Bold;

            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 24;

            GUILayout.BeginHorizontal();
            var c = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Select Scenes in Build", bottomButtonStyle, GUILayout.Height(25), GUILayout.Width(position.size.x / 2)))
            {
                SelectScenesInBuild();
            }

            if (GUILayout.Button("Done", buttonStyle, GUILayout.Height(25), GUILayout.Width(position.size.x / 2)))
            {
                Done();
            }
            GUI.backgroundColor = c;
            GUILayout.EndHorizontal();

            //EditorGUIUtility.IconContent("PlayButton").image
        }

        private static void Done()
        {
            UpdateProjectScenes.GetAllScenes(scenes.Where(ctx => ctx.isSelected).Select(ctx => ctx.name).ToList());
            window.Close();
        }

        private static void SelectScenesInBuild()
        {
            var buildScenes = SceneLoaderUtility.GetScenesInBuild();
            foreach (var scene in scenes)
            {
                scene.isSelected = buildScenes.Contains(scene.name);
            }
        }

        private static void GetScenes()
        {
            scenes.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Scene");
            string[] scenePaths = new string[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                scenePaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[i]);
                var sceneData = new SceneData()
                {
                    name = sceneName,
                    path = scenePaths[i],
                    isSelected = IsInProjectScenes(sceneName)
                };

                scenes.Add(sceneData);
            }
        }

        private static bool IsInProjectScenes(string sceneName)
        {
            return System.Enum.GetNames(typeof(ProjectScenes)).ToList().Contains(sceneName);
        }
    }
}