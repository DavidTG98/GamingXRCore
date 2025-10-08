using UnityEngine;
using System.Reflection;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamingXRCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonLabel { get; private set; }

        public ButtonAttribute(string buttonLabel)
        {
            ButtonLabel = buttonLabel;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonPropertyDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            MonoBehaviour monoBehaviour = (MonoBehaviour)target;
            MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
                foreach (var attribute in attributes)
                {
                    ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
                    if (GUILayout.Button(buttonAttribute.ButtonLabel))
                    {
                        method.Invoke(monoBehaviour, null);
                    }
                }
            }
        }
    }

    [CustomEditor(typeof(ScriptableObject), true)]
    public class ButtonPropertyDrawerScriptable : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            ScriptableObject scriptableObject = (ScriptableObject)target;
            MethodInfo[] methods = scriptableObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
                foreach (var attribute in attributes)
                {
                    ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
                    if (GUILayout.Button(buttonAttribute.ButtonLabel))
                    {
                        method.Invoke(scriptableObject, null);
                    }
                }
            }
        }
    }
#endif
}