using UnityEngine;

namespace GamingXRCore.OpenAIIntegration
{
    [CreateAssetMenu(fileName = "SO_OpenAiModel", menuName = "ScriptableObjects/OpenAiModel")]
    public class SO_OpenAiModel : ScriptableObject
    {
        public string openAiApiKey;
        public string openAiOrganizationId;

        public GPTModel gPTModel = GPTModel.gpt_4o_mini;
        [Range(0, 2)] public float chatTemperature = 0.75f;
        [TextArea(3, 15)] public string chatContext = "";
    }
}