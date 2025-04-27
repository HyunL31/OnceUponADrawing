using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public TextMeshPro numberText;
    private int number;
    private bool isEven;

    private void Start()
    {
        GenerateRandomNumber();
    }

    public void GenerateRandomNumber()
    {
        float evenChance = 0.60f; // 60% Ȯ���� ¦�� ������

        if (Random.value < evenChance)
        {
            int n = Random.Range(1, 50) * 2; // ¦����
            number = n;
        }
        else
        {
            int n = Random.Range(0, 50) * 2 + 1; // Ȧ����
            number = n;
        }

        isEven = (number % 2 == 0);

        if (numberText != null)
        {
            numberText.text = number.ToString();

            // ��濡 ���� �������� ���� ����
            var textRenderer = numberText.GetComponent<Renderer>();

            if (textRenderer != null)
            {
                textRenderer.sortingOrder = 5;
            }
        }
    }

    // �÷��̾�� �ε����� ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isEven)
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
