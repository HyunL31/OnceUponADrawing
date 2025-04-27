using UnityEngine;

public static class DrawingLoader
{
    public static Sprite LoadPlayerDrawing()
    {
        string path = Application.persistentDataPath + "/playerDrawing.png";

        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D originalTex = new Texture2D(2, 2);
            originalTex.LoadImage(bytes);

            // 캐릭터가 실제로 그려진 영역 계산
            Rect trimmedRect = GetBounds(originalTex, out int offsetX, out int offsetY);

            if (trimmedRect.width == 0 || trimmedRect.height == 0)
            {
                return null;
            }

            // 잘라낸 텍스처 만들기
            Texture2D trimmedTex = new Texture2D((int)trimmedRect.width, (int)trimmedRect.height);
            trimmedTex.SetPixels(originalTex.GetPixels(offsetX, offsetY, (int)trimmedRect.width, (int)trimmedRect.height));
            trimmedTex.Apply();

            // pivot을 하단 중심으로 설정
            Vector2 pivot = new Vector2(0.5f, 0f);

            return Sprite.Create(trimmedTex, new Rect(0, 0, trimmedTex.width, trimmedTex.height), pivot);
        }
        else
        {
            return null;
        }
    }

    // 불투명한 영역의 경계 계산 함수
    private static Rect GetBounds(Texture2D tex, out int offsetX, out int offsetY)
    {
        int minX = tex.width, minY = tex.height;
        int maxX = 0, maxY = 0;

        Color[] pixels = tex.GetPixels();

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                Color pixel = pixels[y * tex.width + x];

                if (pixel.a > 0.01f) // 알파가 거의 0보다 크면 그려진 것으로 판단
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
