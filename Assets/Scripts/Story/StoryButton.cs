using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;

/// <summary>
/// ��ȭ ���� ��ư ��ũ��Ʈ
/// �ر� ���ο� ���� ��ư Ȱ��, ��Ȱ��ȭ
/// </summary>

public class StoryButton : MonoBehaviour
{
    // ��ȭ ���� ��ư UI ���
    public int storyID;
    public Image lockIcon;
    public Image taleTitle;
    public Image mainCharacter;
    public Button button;

    private StoryData storyData;

    void Start()
    {
        storyData = StoryDatabase.Instance.GetStoryById(storyID);

        if (storyData == null) return;

        taleTitle.sprite = storyData.titleImage;
        mainCharacter.sprite = storyData.characterImage;

        UpdateUI();

        button.onClick.AddListener(() =>
        {
            if (storyData.isImplemented)
            {
                PlayerPrefs.SetString("CurrentStoryCode", storyData.storyCode);
                SceneManager.LoadScene(storyData.sceneName);
            }
            else
            {
                Debug.Log("���� �������� ���� ���丮");
            }
        });
    }

    // UI ������Ʈ
    public void UpdateUI()
    {
        bool isUnlocked = StoryUnlockManager.Instance.IsStoryUnlocked(storyID);

        lockIcon.gameObject.SetActive(!isUnlocked);
        taleTitle.gameObject.SetActive(isUnlocked);
        mainCharacter.gameObject.SetActive(isUnlocked);
        button.interactable = isUnlocked;
    }
}
