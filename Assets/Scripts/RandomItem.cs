using UnityEngine;
using TMPro; // TextMeshPro�� ����� �Ŷ�� �ʿ�

public class RandomItemType : MonoBehaviour
{
    public Sprite evenSprite;
    public Sprite oddSprite;

    private TextMeshPro text; // ������ �ȿ� �ִ� �ؽ�Ʈ

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();

        if (Random.value < 0.5f)
        {
            // ¦�� ������
            sr.sprite = evenSprite;
            gameObject.tag = "EvenItem";
            if (text != null)
            {
                int evenNumber = RandomEvenNumber(2, 98); // 2~98 ���� ¦��
                text.text = evenNumber.ToString();
            }
        }
        else
        {
            // Ȧ�� ������
            sr.sprite = oddSprite;
            gameObject.tag = "OddItem";
            if (text != null)
            {
                int oddNumber = RandomOddNumber(1, 99); // 1~99 ���� Ȧ��
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
