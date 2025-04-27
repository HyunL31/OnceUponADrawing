using UnityEngine;
using TMPro; // TextMeshPro를 사용할 거라면 필요

public class RandomItemType : MonoBehaviour
{
    public Sprite evenSprite;
    public Sprite oddSprite;

    private TextMeshPro text; // 아이템 안에 있는 텍스트

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();

        if (Random.value < 0.5f)
        {
            // 짝수 아이템
            sr.sprite = evenSprite;
            gameObject.tag = "EvenItem";
            if (text != null)
            {
                int evenNumber = RandomEvenNumber(2, 98); // 2~98 사이 짝수
                text.text = evenNumber.ToString();
            }
        }
        else
        {
            // 홀수 아이템
            sr.sprite = oddSprite;
            gameObject.tag = "OddItem";
            if (text != null)
            {
                int oddNumber = RandomOddNumber(1, 99); // 1~99 사이 홀수
                text.text = oddNumber.ToString();
            }
        }
    }

    int RandomEvenNumber(int min, int max)
    {
        int num = Random.Range(min / 2, max / 2 + 1) * 2;
        return Mathf.Clamp(num, min, max);
    }

    int RandomOddNumber(int min, int max)
    {
        int num = Random.Range((min + 1) / 2, (max + 1) / 2) * 2 - 1;
        return Mathf.Clamp(num, min, max);
    }
}
