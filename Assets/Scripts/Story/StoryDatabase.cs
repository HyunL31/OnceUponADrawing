using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȭ ��� & ���� �����ͺ��̽�
/// </summary>

public class StoryDatabase : MonoBehaviour
{
    public static StoryDatabase Instance;

    public List<StoryData> storyList = new List<StoryData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ID�� ���� ��ȭ ��ȯ
    public StoryData GetStoryById(int id)
    {
        return storyList.Find(story => story.id == id);
    }

    // ��� ��ȭ ����Ʈ ��������
    public List<StoryData> GetAllStories()
    {
        return storyList;
    }
}
