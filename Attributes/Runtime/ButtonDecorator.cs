using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamingXRCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonLabel { get; private set; }

        public ButtonAttribute(string buttonLabel = default)
        {
            ButtonLabel = buttonLabel;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
    public class ButtonPropertyDrawer_MonoBehaviour : ButtonPropertyDrawer<MonoBehaviour> { }


    [CustomEditor(typeof(ScriptableObject), editorForChildClasses: true)]
    public class ButtonPropertyDrawer_ScriptableObject : ButtonPropertyDrawer<ScriptableObject> { }


    public class ButtonPropertyDrawer<T> : Editor where T : UnityEngine.Object
    {
        private T Instance;
        private Type type;
        private readonly List<MethodInfo> allMethods = new List<MethodInfo>();

        private void OnEnable()
        {
            Instance = target as T;
            type = Instance.GetType();

            GetMethods();
        }

        private void GetMethods()
        {
            allMethods.Clear();

            while (type != null && type != typeof(MonoBehaviour))
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                allMethods.AddRange(methods);
                type = type.BaseType;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);

            foreach (MethodInfo method in allMethods)
            {
                var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
                foreach (var attribute in attributes)
                {
                    ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;

                    var label = string.IsNullOrEmpty(buttonAttribute.ButtonLabel) ? method.Name : buttonAttribute.ButtonLabel;
                    if (GUILayout.Button(label))
                    {
                        method.Invoke(Instance, null);
                    }
                }
            }
        }
    }
#endif
}