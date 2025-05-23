using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 동화 별 정보 & 칭호 관련 시스템
/// </summary>

public class FameSystem : MonoBehaviour
{
    public static FameSystem Instance;

    private HashSet<string> unlockedTitles = new HashSet<string>(); // 해금된 칭호 목록

    // 동화 정보 구조
    [System.Serializable]
    public class StoryDataEntry
    {
        public string code;
        public string displayName;
        public Sprite image;
    }

    // 동화 리스트
    [SerializeField]
    public List<StoryDataEntry> storyEntries;

    // 동화 리스트 반환
    public StoryDataEntry GetStoryData(string code)
    {
        return storyEntries.Find(entry => entry.code == code);
    }

    // 칭호 데이터 구조
    [System.Serializable]
    public class TitleData
    {
        public string titleName;
        public Sprite icon;
    }

    // Json 직렬화
    [System.Serializable]
    public class TitleDataEntry
    {
        public string code;
        public string titleName;
        public Sprite icon;
    }

    [SerializeField]
    private List<TitleDataEntry> titleEntries;
    private Dictionary<string, TitleData> titleMap = new Dictionary<string, TitleData>();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("AppInitialized"))
            {
                PlayerPrefs.DeleteKey("UnlockedTitles");
                PlayerPrefs.SetInt("AppInitialized", 1);
                PlayerPrefs.Save();
            }

            LoadTitles();
            InitializeTitleMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 리스트를 dictionary로 변환
    private void InitializeTitleMap()
    {
        titleMap.Clear();

        foreach (var entry in titleEntries)
        {
            titleMap[entry.code] = new TitleData
            {
                titleName = entry.titleName,
                icon = entry.icon
            };
        }
    }

    // 칭호 해금 & 저장
    public void UnlockTitle(string title)
    {
        if (!unlockedTitles.Contains(title))
        {
            unlockedTitles.Add(title);
            SaveTitles();
        }
    }

    public bool HasTitle(string title) => unlockedTitles.Contains(title);

    public List<string> GetAllTitles() => new List<string>(unlockedTitles);

    private void SaveTitles()
    {
        string saved = string.Join(",", unlockedTitles);
        PlayerPrefs.SetString("UnlockedTitles", saved);
    }
    private void LoadTitles()
    {
        string saved = PlayerPrefs.GetString("UnlockedTitles", "");

        if (!string.IsNullOrEmpty(saved))
        {
            unlockedTitles = new HashSet<string>(saved.Split(','));
        }
    }

    // 칭호 아이콘 & 이름 반환
    public TitleData GetTitleData(string code)
    {
        if (titleMap.ContainsKey(code))
        {
            return titleMap[code];
        }

        return null;
    }
}