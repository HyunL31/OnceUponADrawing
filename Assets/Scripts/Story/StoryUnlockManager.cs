using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해금된 동화 정보 저장 & 불러오기
/// </summary>

public class StoryUnlockManager : MonoBehaviour
{
    // 해금된 동화들
    [System.Serializable]
    public class StoryProgress
    {
        public List<int> unlockedStoryIDs = new List<int>();
    }

    // 싱글톤 인스턴스
    private static StoryUnlockManager _instance;

    public static StoryUnlockManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("StoryUnlockManager");
                _instance = obj.AddComponent<StoryUnlockManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private StoryProgress progress;

    // 처음 실행 시 초기화 & 동화 로드
    void Awake()
    {
        if (!PlayerPrefs.HasKey("AppInitialized"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("AppInitialized", 1);
            PlayerPrefs.Save();
        }

        LoadProgress();
    }

    // 저장된 해금 동화 데이터 불러오기
    private void LoadProgress()
    {
        string json = PlayerPrefs.GetString("UnlockedStories", "{}");
        progress = JsonUtility.FromJson<StoryProgress>(json);

        if (progress.unlockedStoryIDs == null)
        {
            progress.unlockedStoryIDs = new List<int>();
        }

        // 빨간 망토는 기본 해금
        if (!progress.unlockedStoryIDs.Contains(1))
        {
            progress.unlockedStoryIDs.Add(1);
        }
    }

    // 동화 해금하고 상태 저장
    public void UnlockStory(int storyID)
    {
        if (!progress.unlockedStoryIDs.Contains(storyID))
        {
            progress.unlockedStoryIDs.Add(storyID);
            SaveProgress();
        }
    }

    // 특정 ID의 동화가 해금되었는지
    public bool IsStoryUnlocked(int storyID)
    {
        return progress.unlockedStoryIDs.Contains(storyID);
    }

    // 해금된 동화 ID 리스트 반환
    public List<int> GetUnlockedStoryIDs()
    {
        return new List<int>(progress.unlockedStoryIDs);
    }

    // 해금된 동화 코드 리스트 반환
    public List<string> GetUnlockedStoryCodes()
    {
        List<string> codes = new List<string>();

        foreach (int id in progress.unlockedStoryIDs)
        {
            switch (id)
            {
                case 1:
                    codes.Add("RedHood");
                    break;
                case 2:
                    codes.Add("Mermaid");
                    break;
                default:
                    codes.Add($"Story{id}");
                    break;
            }
        }

        return codes;
    }

    // 해금된 동화 이름 리스트 반환
    public List<string> GetUnlockedStoryNames()
    {
        List<string> names = new List<string>();

        foreach (int id in progress.unlockedStoryIDs)
        {
            switch (id)
            {
                case 1:
                    names.Add("빨간 망토");
                    break;
                case 2:
                    names.Add("인어공주");
                    break;
                default:
                    names.Add($"동화 {id}번");
                    break;
            }
        }

        return names;
    }

    // 현재 해금 상태 저장
    private void SaveProgress()
    {
        string json = JsonUtility.ToJson(progress);
        PlayerPrefs.SetString("UnlockedStories", json);
        PlayerPrefs.Save();
    }
}
