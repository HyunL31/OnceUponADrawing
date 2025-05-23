using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 마이페이지 저장 관리 스크립트
/// </summary>

[System.Serializable]
public class MyPageEntry
{
    public string storyCode;
    public string personality;
}

[System.Serializable]
public class MyPageEntryList
{
    public List<MyPageEntry> entries = new();
}

public static class MyPageSaveManager
{
    // 파일 저장 경로
    private static string path => Path.Combine(Application.persistentDataPath, "mypage_data.json");

    public static void SaveEntry(MyPageEntry newEntry)
    {
        MyPageEntryList wrapper = LoadAll();

        // 중복 여부 확인
        bool alreadyExists = wrapper.entries.Exists(entry =>
            entry.storyCode == newEntry.storyCode &&
            entry.personality == newEntry.personality
        );

        if (!alreadyExists)
        {
            wrapper.entries.Add(newEntry);

            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(path, json);
        }
    }

    // 새로운 엔트리 추가하고 리스트 저장
    public static MyPageEntryList LoadAll()
    {
        if (!File.Exists(path))
        {
            return new MyPageEntryList();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<MyPageEntryList>(json);
    }

    // 저장 파일 완전 삭제 (초기화)
    public static void ClearAll()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
