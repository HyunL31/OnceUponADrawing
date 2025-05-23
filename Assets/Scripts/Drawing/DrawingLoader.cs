using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DrawingSystem;

/// <summary>
/// 그린 플레이어 캐릭터를 로드
/// </summary>

public static class DrawingLoader
{
    // 캐릭터 png 로드
    public static Sprite LoadPlayerDrawing()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerDrawing.png");

        return LoadSpriteFromPath(path);
    }

    // 경로에서 불러온 png를 sprite로 변환
    public static Sprite LoadSpriteFromPath(string path)
    {
        // 바이트로 읽어오기
        byte[] bytes = File.ReadAllBytes(path);

        Texture2D originalTex = new Texture2D(2, 2);
        originalTex.LoadImage(bytes);

        // 캐릭터 경계 설정
        Rect trimmedRect = GetBounds(originalTex, out int offsetX, out int offsetY);

        if (trimmedRect.width == 0 || trimmedRect.height == 0)
        {
            return null;
        }

        // 경계를 기반으로 새 텍스처 생성
        Texture2D trimmedTex = new Texture2D((int)trimmedRect.width, (int)trimmedRect.height);
        trimmedTex.SetPixels(originalTex.GetPixels(offsetX, offsetY, (int)trimmedRect.width, (int)trimmedRect.height));
        trimmedTex.Apply();

        // sprite로 변환
        // pivot은 하단 중앙
        return Sprite.Create(trimmedTex, new Rect(0, 0, trimmedTex.width, trimmedTex.height), new Vector2(0.5f, 0f));
    }

    // 투명하지 않은 픽셀을 기준으로 캐릭터 범위 계산
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

                if (pixel.a > 0.01f)
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