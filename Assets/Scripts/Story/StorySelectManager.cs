using UnityEngine;

/// <summary>
/// 동화 선택창에서 버튼 생성 스크립트
/// </summary>

public class StorySelectManager : MonoBehaviour
{
    public Transform contentParent;

    public GameObject storyButtonPrefab;

    void Start()
    {
        if (StoryDatabase.Instance == null)
        {
            return;
        }

        var stories = StoryDatabase.Instance.GetAllStories();

        foreach (var story in StoryDatabase.Instance.GetAllStories())
        {
            GameObject btnObj = Instantiate(storyButtonPrefab, contentParent);
            StoryButton btn = btnObj.GetComponent<StoryButton>();

            // 버튼에 스토리 정보 할당
            btn.storyID = story.id;
            btn.taleTitle.sprite = story.titleImage;
            btn.mainCharacter.sprite = story.characterImage;
            btn.lockIcon.sprite = GetLockIcon();

            btn.UpdateUI();
        }
    }

    // 해금되지 않은 동화용 자물쇠 아이콘
    private Sprite GetLockIcon()
    {
        Sprite icon = Resources.Load<Sprite>("Icons/Lock");

        return icon;
    }
}
