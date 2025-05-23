using TMPro;
using UnityEngine;

/// <summary>
/// È¦¼ö, Â¦¼ö »ç°ú
/// </summary>

public class Item : MonoBehaviour
{
    public TextMeshPro numberText;

    [Header("Odd || Even")]
    public bool isEvenItem = true;

    private int number;

    private void Start()
    {
        GenerateNumber();
    }

    // È¦¼ö Â¦¼ö ·£´ýÀ¸·Î ¹Þ¾Æ¿À±â
    private void GenerateNumber()
    {
        if (isEvenItem)
        {
            number = Random.Range(1, 50) * 2; // Â¦¼ö: 2~98
        }
        else
        {
            number = Random.Range(0, 50) * 2 + 1; // È¦¼ö: 1~99
        }

        if (numberText != null)
        {
            numberText.text = number.ToString();

            var textRenderer = numberText.GetComponent<Renderer>();

            if (textRenderer != null)
            {
                textRenderer.sortingOrder = 5;
            }
        }
    }

    // ÇÃ·¹ÀÌ¾î¿Í ´ê¾ÒÀ» ¶§
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isEvenItem)
            {
                GameManager.Instance.AddScore(1);
            }
            else
            {
                GameManager.Instance.HitOddItem();
            }

            Destroy(gameObject);
        }
    }
}
