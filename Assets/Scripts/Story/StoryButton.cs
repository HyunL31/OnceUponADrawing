using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;

/// <summary>
/// 동화 선택 버튼 스크립트
/// 해금 여부에 따라 버튼 활성, 비활성화
/// </summary>

public class StoryButton : MonoBehaviour
{
    // 동화 선택 버튼 UI 요소
    public int storyID;
    public Image lockIcon;
    public Image taleTitle;
    public Image mainCharacter;
    public Button button;

    // 동화 데이터
    [System.Serializable]
    public class StoryData
    {
        public int id;
        public string storyCode;
        public string storyName; 
        public string sceneName;
        public Sprite titleImage;
        public Sprite characterImage;
        public bool isImplemented; 
    }

    private StoryData storyData;

    void Start()
    {
        // 스토리 DB에서 동화 가져오기
        storyData = StoryDatabase.Instance.GetStoryById(storyID);

        if (storyData == null)
        {
            return;
        }

        // 버튼 UI 설정
        taleTitle.sprite = storyData.titleImage;
        mainCharacter.sprite = storyData.characterImage;

        UpdateUI();

        button.onClick.AddListener(() => {
            if (storyData.isImplemented)
            {
                PlayerPrefs.SetString("CurrentStoryCode", storyData.storyCode);

                if (storyData.storyCode == "RedHood")
                {
                    SceneManager.LoadScene(storyData.sceneName);
                }
            }
            else
            {
                Debug.Log("아직 구현되지 않은 스토리");
            }
        });
    }

    // UI 업데이트
    public void UpdateUI()
    {
        bool isUnlocked = StoryUnlockManager.Instance.IsStoryUnlocked(storyID);

        lockIcon.gameObject.SetActive(!isUnlocked);
        taleTitle.gameObject.SetActive(isUnlocked);
        mainCharacter.gameObject.SetActive(isUnlocked);
        button.interactable = isUnlocked;
    }
}
