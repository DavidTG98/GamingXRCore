using UnityEngine;

namespace GamingXRCore.Attributes
{
    public class DelaySpace : PropertyAttribute
    {
        public int space;

        public DelaySpace(int level = 1)
        {
            this.space = level;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(DelaySpace))]
    public class DelaySpaceDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            var space = (attribute as DelaySpace).space;
            return UnityEditor.EditorGUI.GetPropertyHeight(property, label, true) + space;
        }

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.PropertyField(position, property, label, true);
        }
    }
#endif
}