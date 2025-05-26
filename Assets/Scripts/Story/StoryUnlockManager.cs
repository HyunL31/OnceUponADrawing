using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �رݵ� ��ȭ ���� ���� & �ҷ�����
/// </summary>

public class StoryUnlockManager : MonoBehaviour
{
    // �رݵ� ��ȭ��
    [System.Serializable]
    public class StoryProgress
    {
        public List<int> unlockedStoryIDs = new List<int>();
    }

    // �̱��� �ν��Ͻ�
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

    // ó�� ���� �� �ʱ�ȭ & ��ȭ �ε�
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

    // ����� �ر� ��ȭ ������ �ҷ�����
    private void LoadProgress()
    {
        string json = PlayerPrefs.GetString("UnlockedStories", "{}");
        progress = JsonUtility.FromJson<StoryProgress>(json);

        if (progress.unlockedStoryIDs == null)
        {
            progress.unlockedStoryIDs = new List<int>();
        }

        // ���� ����� �⺻ �ر�
        if (!progress.unlockedStoryIDs.Contains(1))
        {
            progress.unlockedStoryIDs.Add(1);
        }
    }

    // ��ȭ �ر��ϰ� ���� ����
    public void UnlockStory(int storyID)
    {
        if (!progress.unlockedStoryIDs.Contains(storyID))
        {
            progress.unlockedStoryIDs.Add(storyID);
            SaveProgress();
        }
    }

    // Ư�� ID�� ��ȭ�� �رݵǾ�����
    public bool IsStoryUnlocked(int storyID)
    {
        return progress.unlockedStoryIDs.Contains(storyID);
    }

    // �رݵ� ��ȭ ID ����Ʈ ��ȯ
    public List<int> GetUnlockedStoryIDs()
    {
        return new List<int>(progress.unlockedStoryIDs);
    }

    // �رݵ� ��ȭ �ڵ� ����Ʈ ��ȯ
    public List<string> GetUnlockedStoryCodes()
    {
        List<string> codes = new List<string>();

        foreach (int id in progress.unlockedStoryIDs)
        {
            var data = StoryDatabase.Instance.GetStoryById(id);

            if (data != null)
            {
                codes.Add(data.storyCode);
            }
        }

        return codes;
    }

    // �رݵ� ��ȭ �̸� ����Ʈ ��ȯ
    public List<string> GetUnlockedStoryNames()
    {
        List<string> names = new List<string>();

        foreach (int id in progress.unlockedStoryIDs)
        {
            var data = StoryDatabase.Instance.GetStoryById(id);

            if (data != null)
            {
                names.Add(data.storyName);
            }
        }

        return names;
    }

    // ���� �ر� ���� ����
    private void SaveProgress()
    {
        string json = JsonUtility.ToJson(progress);
        PlayerPrefs.SetString("UnlockedStories", json);
        PlayerPrefs.Save();
    }
}
