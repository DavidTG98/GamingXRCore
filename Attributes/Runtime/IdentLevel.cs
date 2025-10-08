using UnityEngine;

namespace GamingXRCore.Attributes
{
    public class IdentLevel : PropertyAttribute
    {
        public int level;

        public IdentLevel(int level = 1)
        {
            this.level = level;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(IdentLevel))]
    public class IdentLevelDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            return UnityEditor.EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            int level = (attribute as IdentLevel).level;
            UnityEditor.EditorGUI.indentLevel += level;
            UnityEditor.EditorGUI.PropertyField(position, property, label, true);
            UnityEditor.EditorGUI.indentLevel -= level;
        }
    }
#endif
}