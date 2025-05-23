using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 동화 버튼 UI를 구성
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
        // 성격 아이콘
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        }

        // 동화 제목 텍스트
        if (titleText != null)
        {
            titleText.text = title;
        }

        // 동화 썸네일
        if (storyImage != null)
        {
            storyImage.sprite = image;
            storyImage.gameObject.SetActive(image != null);
        }

        // 성격 설명 텍스트
        if (subText != null)
        {
            subText.text = desc;
        }

        // 성격 이름 텍스트
        if (personalityText != null)
        {
            personalityText.text = personality;
        }
    }
}
