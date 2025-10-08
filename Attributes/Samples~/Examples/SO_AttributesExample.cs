using UnityEngine;

namespace GamingXRCore.Attributes
{
    [CreateAssetMenu(fileName = "SO_AttributesExample", menuName = "SO_AttributesExample")]
    public class SO_AttributesExample : ScriptableObject
    {
        [IdentLevel] public float identVariable = 12.5f;
        [DelaySpace(20)] public Vector2 delayedSpaceVariable = new Vector2(1, 2);
        [ReadOnly] public string readonlyVariable = "Unclickable text";
        [PreviewSprite(150)] public Sprite spriteVariable;

        [Button("SimulateButton")]
        private void SimulateButton()
        {
            Debug.Log("Button was clicked!");
        }
    }
}