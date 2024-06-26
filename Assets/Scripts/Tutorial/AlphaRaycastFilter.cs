using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlphaRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    private Image image;
    private Texture2D texture;

    void Awake()
    {
        image = GetComponent<Image>();

        // Ensure the image has a sprite
        if (image.sprite != null)
        {
            // Get the texture from the sprite
            texture = image.sprite.texture;
        }
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (texture == null)
            return false;

        RectTransform rectTransform = transform as RectTransform;
        Vector2 localPoint;

        // Convert screen point to local point in the RectTransform
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint))
            return false;

        // Normalize local point to texture coordinates
        Rect rect = rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;

        // Convert normalized coordinates to texture coordinates
        int texX = Mathf.RoundToInt(x * texture.width);
        int texY = Mathf.RoundToInt(y * texture.height);

        // Check if texture coordinates are within the bounds of the texture
        if (texX < 0 || texX >= texture.width || texY < 0 || texY >= texture.height)
            return false;

        // Get the alpha value of the pixel
        Color pixelColor = texture.GetPixel(texX, texY);
        return pixelColor.a > 0.1f; // Adjust the threshold as needed
    }
}
