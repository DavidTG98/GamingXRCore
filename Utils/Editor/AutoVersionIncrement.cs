#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GamingXRCore.Utils
{
    public class AutoVersionIncrement : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // Only run for WebGL builds
            if (report.summary.platform != BuildTarget.WebGL)
                return;

            // Obtém a versão atual (ex: "1.0.3")
            string currentVersion = PlayerSettings.bundleVersion;

            // Divide por pontos
            string[] parts = currentVersion.Split('.');
            int major = int.Parse(parts[0]);
            int minor = int.Parse(parts[1]);
            int patch = int.Parse(parts[2]);

            // Incrementa o patch
            patch++;

            // Atualiza a versão
            string newVersion = $"{major}.{minor}.{patch}";
            PlayerSettings.bundleVersion = newVersion;

            // Salva no projeto
            AssetDatabase.SaveAssets();
            Debug.Log($"Versão incrementada automaticamente: {currentVersion} -> {newVersion}");
        }
    }
}
#endif