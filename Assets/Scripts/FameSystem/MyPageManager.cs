using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마이페이지 (동화 별 성격 칭호 시스템)
/// </summary>

public class MyPageManager : MonoBehaviour
{
    public FameSystem fameSystem;

    public Transform itemListParent;
    public GameObject infoItemPrefab;

    public GameObject titlePanel;

    void Start()
    {
        AddStoryItems();
    }

    // 플레이어가 진행한 동화마다 Info Item UI 생성
    void AddStoryItems()
    {
        List<string> storyCodes = StoryUnlockManager.Instance.GetUnlockedStoryCodes();

        foreach (string code in storyCodes)
        {
            var storyData = fameSystem.GetStoryData(code);

            if (storyData == null)
            {
                continue;
            }

            // 동화별 성격 가져오기
            string personality = PlayerPrefs.GetString($"StoryPersonality{code}", "Kind");      // 기본값 Kind

            // 성격 기반 칭호
            var titleData = fameSystem.GetTitleData($"{personality}");

            // 칭호 이름
            string description = personality switch
            {
                "Kind" => "따뜻한 마음의 수호자",
                "Brave" => "두려움 없는 숲의 용사",
                "Curious" => "끝없는 호기심의 탐험가",
                _ => "알 수 없는 성격"
            };

            // 성격 설명 텍스트
            string subText = personality switch
            {
                "Kind" => "이야기 속 당신은 다정하고 배려 깊은 인물이었어요.",
                "Brave" => "위험 앞에서도 용기 내는 모습이 인상 깊었답니다.",
                "Curious" => "무엇이든 궁금해하며 세상을 배우려는 당신의 눈빛!",
                _ => "알 수 없는 성격"
            };

            // 동화 썸네일 & 성격 아이콘
            Sprite storyTitle = Resources.Load<Sprite>($"Icons/{code}");
            Sprite icon = Resources.Load<Sprite>($"Icons/{personality}");

            AddInfoItem(storyTitle, storyData.displayName, icon, description, subText);
        }
    }

    void AddInfoItem(Sprite storyImage, string storyTitle, Sprite personalIcon, string personalTitle, string subText)
    {
        GameObject obj = Instantiate(infoItemPrefab, itemListParent);
        InfoItem item = obj.GetComponent<InfoItem>();

        if (item != null)
        {
            item.Init(
                icon: personalIcon,
                title: storyTitle,
                image: storyImage,
                desc: subText,
                personality: personalTitle
            );
        }
    }

    // 버튼 컨트롤
    public void Back()
    {
        titlePanel.SetActive(false);
    }

    public void Title()
    {
        titlePanel.SetActive(true);
    }
}
