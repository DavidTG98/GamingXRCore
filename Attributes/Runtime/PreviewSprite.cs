using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GamingXRCore.Attributes
{
    public class PreviewSprite : PropertyAttribute
    {
        public float imageSize;
        public PreviewSprite(float imageSize)
        {
            this.imageSize = imageSize;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PreviewSprite))]
    public class PreviewSpriteDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                (property.objectReferenceValue as Sprite) != null)
            {
                return EditorGUI.GetPropertyHeight(property, label, true) + (attribute as PreviewSprite).imageSize;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PreviewSprite thisAttribute = attribute as PreviewSprite;

            // Draw the normal property field
            EditorGUI.PropertyField(position, property, label, true);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Sprite sprite = property.objectReferenceValue as Sprite;
                if (sprite != null)
                {
                    position.y += EditorGUI.GetPropertyHeight(property, label, true) + 5;
                    position.height = thisAttribute.imageSize;

                    // Draw the sprite portion using GUI with texture coordinates
                    DrawSpritePreview(position, sprite);
                }
            }
        }

        static void DrawSpritePreview(Rect position, Sprite sprite)
        {
            Rect spriteRect = sprite.textureRect;
            Texture2D texture = sprite.texture;

            // Calculate UV coordinates for the sprite within the atlas
            Rect uvRect = new Rect(
                spriteRect.x / texture.width,
                spriteRect.y / texture.height,
                spriteRect.width / texture.width,
                spriteRect.height / texture.height
            );

            // Calculate aspect ratio and adjust rect to maintain it
            float spriteAspect = spriteRect.width / spriteRect.height;
            float rectAspect = position.width / position.height;

            Rect drawRect = position;

            if (spriteAspect > rectAspect)
            {
                // Sprite is wider - fit to width, adjust height
                float newHeight = position.width / spriteAspect;
                drawRect.y += (position.height - newHeight) / 2;
                drawRect.height = newHeight;
            }
            else
            {
                // Sprite is taller - fit to height, adjust width
                float newWidth = position.height * spriteAspect;
                drawRect.x += (position.width - newWidth) / 2;
                drawRect.width = newWidth;
            }

            // Draw only the sprite portion using UV coordinates
            GUI.DrawTextureWithTexCoords(drawRect, texture, uvRect);
        }
    }
#endif
}