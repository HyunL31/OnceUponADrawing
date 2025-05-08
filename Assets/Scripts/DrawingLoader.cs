using UnityEngine;

/// <summary>
/// Loading Player Character
/// </summary>

public static class DrawingLoader
{
    public static Sprite LoadPlayerDrawing()
    {
        string path = Application.persistentDataPath + "/playerDrawing.png";

        // Get character texture from path of file
        if (System.IO.File.Exists(path))
        {
            // Get texture from path of file
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D originalTex = new Texture2D(2, 2);
            originalTex.LoadImage(bytes);       // Convert to texture from PNG

            Rect trimmedRect = GetBounds(originalTex, out int offsetX, out int offsetY);

            if (trimmedRect.width == 0 || trimmedRect.height == 0)
            {
                return null;
            }

            // Create cut texture
            Texture2D trimmedTex = new Texture2D((int)trimmedRect.width, (int)trimmedRect.height);
            trimmedTex.SetPixels(originalTex.GetPixels(offsetX, offsetY, (int)trimmedRect.width, (int)trimmedRect.height));
            trimmedTex.Apply();

            // Setting pivot
            Vector2 pivot = new Vector2(0.5f, 0f);

            return Sprite.Create(trimmedTex, new Rect(0, 0, trimmedTex.width, trimmedTex.height), pivot);
        }
        else
        {
            return null;
        }
    }

    // Get bound for character
    private static Rect GetBounds(Texture2D tex, out int offsetX, out int offsetY)
    {
        // Initialize
        int minX = tex.width, minY = tex.height;
        int maxX = 0, maxY = 0;

        // Get entire of character pixels
        Color[] pixels = tex.GetPixels();

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                Color pixel = pixels[y * tex.width + x];

                if (pixel.a > 0.01f)        // if alpha is bigger than 0, there is a drawing pixel
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        offsetX = minX;
        offsetY = minY;

        return new Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }
}
