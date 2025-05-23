using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ȭ ��ư UI�� ����
/// </summary>

public class InfoItem : MonoBehaviour
{
    public Image storyImage;
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subText;
    public TextMeshProUGUI personalityText;

    public void Init(Sprite icon, string title, Sprite image = null, string desc = "", string personality = "")
    {
        // ���� ������
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        }

        // ��ȭ ���� �ؽ�Ʈ
        if (titleText != null)
        {
            titleText.text = title;
        }

        // ��ȭ �����
        if (storyImage != null)
        {
            storyImage.sprite = image;
            storyImage.gameObject.SetActive(image != null);
        }

        // ���� ���� �ؽ�Ʈ
        if (subText != null)
        {
            subText.text = desc;
        }

        // ���� �̸� �ؽ�Ʈ
        if (personalityText != null)
        {
            personalityText.text = personality;
        }
    }
}
