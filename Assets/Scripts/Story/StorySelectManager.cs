using UnityEngine;

/// <summary>
/// ��ȭ ����â���� ��ư ���� ��ũ��Ʈ
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

            // ��ư�� ���丮 ���� �Ҵ�
            btn.storyID = story.id;
            btn.taleTitle.sprite = story.titleImage;
            btn.mainCharacter.sprite = story.characterImage;
            btn.lockIcon.sprite = GetLockIcon();

            btn.UpdateUI();
        }
    }

    // �رݵ��� ���� ��ȭ�� �ڹ��� ������
    private Sprite GetLockIcon()
    {
        Sprite icon = Resources.Load<Sprite>("Icons/Lock");

        return icon;
    }
}
