using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drawing System Script
/// </summary>

public class DrawingSystem : MonoBehaviour
{
    public RawImage targetImage;
    public int width = 800;
    public int height = 800;

    public Color drawColor;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image previewColor;

    public int brushSize = 10;
    public TextMeshProUGUI sizeText;

    private Texture2D drawingTexture;
    private bool isDrawing = false;
    private Vector2 previousPos;

    public static DrawingSystem Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        drawingTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        drawingTexture.filterMode = FilterMode.Point;       // Fixel Mode
        targetImage.texture = drawingTexture;
        ClearScreen();
    }

    void Update()
    {
        Vector2 screenPos;

        if (Input.touchCount > 0)       // for mobile
        {
            screenPos = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))       // for editor (debugging)
        {
            screenPos = Input.mousePosition;
        }
        else
        {
            isDrawing = false;
            return;
        }

        // Convert to screen position from UI position
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(targetImage.rectTransform, screenPos, null, out Vector2 localPoint))
        {
            return;
        }

        // Calculate pixel size
        float px = targetImage.rectTransform.rect.width / width;
        float py = targetImage.rectTransform.rect.height / height;

        // Convert to texture position from local position
        Vector2 pixelPos = new Vector2(
            (localPoint.x + targetImage.rectTransform.rect.width / 2) / px,
            (localPoint.y + targetImage.rectTransform.rect.height / 2) / py
        );

        // Starting point
        if (!isDrawing)
        {
            previousPos = pixelPos;
            isDrawing = true;
        }

        DrawLine(previousPos, pixelPos);
        previousPos = pixelPos;
    }

    // Bresenham algorithm
    private void DrawLine(Vector2 start, Vector2 end)
    {
        int x0 = Mathf.RoundToInt(start.x);
        int y0 = Mathf.RoundToInt(start.y);
        int x1 = Mathf.RoundToInt(end.x);
        int y1 = Mathf.RoundToInt(end.y);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
            {
                DrawPixelBlock(x0, y0, brushSize);
            }

            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        drawingTexture.Apply();
    }

    // Fixel block
    private void DrawPixelBlock(int centerX, int centerY, int size)
    {
        int half = size / 2;

        for (int x = -half; x <= half; x++)
        {
            for (int y = -half; y <= half; y++)
            {
                int px = centerX + x;
                int py = centerY + y;

                if (px >= 0 && px < width && py >= 0 && py < height)
                {
                    drawingTexture.SetPixel(px, py, drawColor);
                }
            }
        }
    }

    // Initialize screen
    public void ClearScreen()
    {
        Color[] clear = new Color[width * height];

        for (int i = 0; i < clear.Length; i++)
        {
            clear[i] = Color.clear;
        }

        drawingTexture.SetPixels(clear);
        drawingTexture.Apply();
    }

    // Manipulate brush size
    public void SetBrushSize(float value)
    {
        brushSize = Mathf.RoundToInt(value);

        if (sizeText != null)
        {
            sizeText.text = $"Brush Size: {brushSize}";
        }
    }

    // Selecting color
    public void ColorChoice()
    {
        float r = redSlider.value;
        float g = greenSlider.value;
        float b = blueSlider.value;

        drawColor = new Color(r, g, b, 1f);

        if (previewColor != null)
        {
            previewColor.color = drawColor;
        }
    }

    // If player press the confirm button, save the character
    public void SaveDrawing()
    {
        byte[] bytes = drawingTexture.EncodeToPNG();
        string path = Application.persistentDataPath + "/playerDrawing.png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Saved to: " + path);
    }
}