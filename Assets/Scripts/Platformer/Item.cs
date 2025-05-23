using TMPro;
using UnityEngine;

/// <summary>
/// Ȧ��, ¦�� ���
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

    // Ȧ�� ¦�� �������� �޾ƿ���
    private void GenerateNumber()
    {
        if (isEvenItem)
        {
            number = Random.Range(1, 50) * 2; // ¦��: 2~98
        }
        else
        {
            number = Random.Range(0, 50) * 2 + 1; // Ȧ��: 1~99
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

    // �÷��̾�� ����� ��
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
