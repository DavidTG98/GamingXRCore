#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace GamingXRCore.SceneLoader
{
    internal class MenuItem_SceneShortcut
    {
        private static void LoadScene(string sceneName)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(sceneName);
            else
                EditorSceneManager.OpenScene(sceneName);
        }

        [MenuItem("GamingXRCore/SceneLoader/Play")]
        private static void PlayFromFirst()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            if (sceneCount > 0)
            {
                // Get the first scene path in build settings
                string scenePath = SceneUtility.GetScenePathByBuildIndex(0);

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("No scenes found in the build settings.");
            }
        }
    }

}
#endif