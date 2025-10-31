using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamingXRCore.SpreadsheetIntegration
{
    public class SpreadsheetIntegrationExample : MonoBehaviour
    {
        [SerializeField] private string webAppUrl = "https://script.google.com/macros/s/AKfycbzLTYE_K-7epG3JRyvwuxxpht5y4WDyOn0g5PMNGA4bjBmfU_5_oFUaSoAhV5NTcUkk/exec";
        [SerializeField] private SpreadsheetTabs tab;
        [SerializeField] private StringTable spreadsheet;
        private bool requestIsInProgress;

        private enum SpreadsheetTabs { Schools, Classes, Students }

        private void ReadSpreadsheet()
        {
            requestIsInProgress = true;
            SpreadsheetIntegration.ReadSheet(this, webAppUrl, tab.ToString(), OnSuccess);

            void OnSuccess(List<List<string>> list)
            {
                requestIsInProgress = false;

                if (list == null)
                    return;

                spreadsheet = StringTable.FromList(list);
            }
        }

        private void WriteSpreadsheet()
        {
            requestIsInProgress = true;
            SpreadsheetIntegration.WriteSheet(this, webAppUrl, tab.ToString(), spreadsheet.ToList(), OnSuccess);

            void OnSuccess(bool obj)
            {
                requestIsInProgress = false;
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(SpreadsheetIntegrationExample))]
        private class Editor_GoogleIntegrationExample : Editor
        {
            private SpreadsheetIntegrationExample script;
            private GUIStyle bottomButtonStyle;
            private Color c1 = new Color(.17f, .85f, 0.095f, 1);
            private Color c2 = new Color(.2f, .75f, 1, 1);

            private void OnEnable()
            {
                script = target as SpreadsheetIntegrationExample;
            }

            private void InitStyles()
            {
                if (bottomButtonStyle != null)
                    return;

                bottomButtonStyle = new GUIStyle(GUI.skin.button);
                bottomButtonStyle.fontSize = 24;
            }

            public override void OnInspectorGUI()
            {
                if (script.requestIsInProgress)
                {
                    GUI.enabled = false;
                }

                InitStyles();

                base.OnInspectorGUI();

                GUILayout.BeginHorizontal();
                var color = GUI.color;
                GUI.backgroundColor = c1;
                if (GUILayout.Button("READ", bottomButtonStyle, GUILayout.Height(28)))
                {
                    script.ReadSpreadsheet();
                }

                GUI.backgroundColor = c2;
                if (GUILayout.Button("WRITE", bottomButtonStyle, GUILayout.Height(28)))
                {
                    script.WriteSpreadsheet();
                }
                GUI.backgroundColor = color;
                GUILayout.EndHorizontal();

                GUI.enabled = true;

                if (script.requestIsInProgress)
                {
                    EditorGUILayout.HelpBox("Request is in progress", MessageType.Warning);
                }
            }
        }
#endif
    }
}