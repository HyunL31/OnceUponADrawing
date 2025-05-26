using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 동화 목록 & 정보 데이터베이스
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

    // ID에 따른 동화 반환
    public StoryData GetStoryById(int id)
    {
        return storyList.Find(story => story.id == id);
    }

    // 모든 동화 리스트 가져오기
    public List<StoryData> GetAllStories()
    {
        return storyList;
    }
}
