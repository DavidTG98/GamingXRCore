using UnityEngine;

namespace GamingXRCore.Tooltip
{
    [CreateAssetMenu(fileName = "TooltipModel", menuName = "GamingXRCore/Tooltip/TooltipModel")]
    public class TooltipModel : ScriptableObject
    {
        [TextArea(2, 10)] public string header;
        [TextArea(5, 10)] public string content;
    }
}