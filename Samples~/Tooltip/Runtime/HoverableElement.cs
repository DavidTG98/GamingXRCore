using UnityEditor;
using UnityEngine;

namespace GamingXRCore.Tooltip
{
    internal class HoverableElement : MonoBehaviour, IHoverable
    {
        [SerializeField] private TooltipModel tooltipModel;

        public TooltipModel GetTooltipModel()
        {
            return tooltipModel;
        }

        void IHoverable.OnHoverEnter() { }
        void IHoverable.OnHoverExit() { }

#if UNITY_EDITOR
        [GamingXRCore.Attributes.Button()]
        public void CreateTooltipModelAsset()
        {
            var newModel = ScriptableObject.CreateInstance<TooltipModel>();

            AssetDatabase.CreateAsset(newModel, "Assets/NewTooltipModel.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newModel;

            tooltipModel = newModel;

        }
#endif
    }
}