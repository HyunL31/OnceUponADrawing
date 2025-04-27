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
        float evenChance = 0.60f; // 60% 확률로 짝수 나오게

        if (Random.value < evenChance)
        {
            int n = Random.Range(1, 50) * 2; // 짝수만
            number = n;
        }
        else
        {
            int n = Random.Range(0, 50) * 2 + 1; // 홀수만
            number = n;
        }

        isEven = (number % 2 == 0);

        if (numberText != null)
        {
            numberText.text = number.ToString();

            // 배경에 숫자 가려지는 현상 방지
            var textRenderer = numberText.GetComponent<Renderer>();

            if (textRenderer != null)
            {
                textRenderer.sortingOrder = 5;
            }
        }
    }

    // 플레이어와 부딪히면 제거
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
