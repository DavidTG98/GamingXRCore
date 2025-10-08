using UnityEngine;

# if UNITY_EDITOR
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
        //const float imageHeight = 100;
        private Texture2D spriteTexture;

        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                (property.objectReferenceValue as Sprite) != null)
            {
                return EditorGUI.GetPropertyHeight(property, label, true) + (attribute as PreviewSprite).imageSize;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        static string GetPath(SerializedProperty property)
        {
            string path = property.propertyPath;
            int index = path.LastIndexOf(".");
            return path.Substring(0, index + 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PreviewSprite thisAttribute = attribute as PreviewSprite;

            //Draw the normal property field
            EditorGUI.PropertyField(position, property, label, true);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Sprite sprite = property.objectReferenceValue as Sprite;
                if (sprite != null)
                {
                    position.y += EditorGUI.GetPropertyHeight(property, label, true) + 5;
                    position.height = thisAttribute.imageSize;

                    if (spriteTexture == null)
                        spriteTexture = TryGetTextureFromAtlas(sprite);

                    GUI.DrawTexture(position, spriteTexture, ScaleMode.ScaleToFit);
                }
            }

            static Texture2D TryGetTextureFromAtlas(Sprite sprite)
            {
                try
                {
                    Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

                    Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                          (int)sprite.textureRect.y,
                                                          (int)sprite.textureRect.width,
                                                          (int)sprite.textureRect.height);
                    croppedTexture.SetPixels(pixels);
                    croppedTexture.Apply();
                    return croppedTexture;
                }
                catch
                {
                    return sprite.texture;
                }
            }
        }
    }
#endif
}