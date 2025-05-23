using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DrawingSystem;

/// <summary>
/// �׸� �÷��̾� ĳ���͸� �ε�
/// </summary>

public static class DrawingLoader
{
    // ĳ���� png �ε�
    public static Sprite LoadPlayerDrawing()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerDrawing.png");

        return LoadSpriteFromPath(path);
    }

    // ��ο��� �ҷ��� png�� sprite�� ��ȯ
    public static Sprite LoadSpriteFromPath(string path)
    {
        // ����Ʈ�� �о����
        byte[] bytes = File.ReadAllBytes(path);

        Texture2D originalTex = new Texture2D(2, 2);
        originalTex.LoadImage(bytes);

        // ĳ���� ��� ����
        Rect trimmedRect = GetBounds(originalTex, out int offsetX, out int offsetY);

        if (trimmedRect.width == 0 || trimmedRect.height == 0)
        {
            return null;
        }

        // ��踦 ������� �� �ؽ�ó ����
        Texture2D trimmedTex = new Texture2D((int)trimmedRect.width, (int)trimmedRect.height);
        trimmedTex.SetPixels(originalTex.GetPixels(offsetX, offsetY, (int)trimmedRect.width, (int)trimmedRect.height));
        trimmedTex.Apply();

        // sprite�� ��ȯ
        // pivot�� �ϴ� �߾�
        return Sprite.Create(trimmedTex, new Rect(0, 0, trimmedTex.width, trimmedTex.height), new Vector2(0.5f, 0f));
    }

    // �������� ���� �ȼ��� �������� ĳ���� ���� ���
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