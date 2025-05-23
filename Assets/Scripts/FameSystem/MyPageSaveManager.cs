using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ���������� ���� ���� ��ũ��Ʈ
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
    // ���� ���� ���
    private static string path => Path.Combine(Application.persistentDataPath, "mypage_data.json");

    public static void SaveEntry(MyPageEntry newEntry)
    {
        MyPageEntryList wrapper = LoadAll();

        // �ߺ� ���� Ȯ��
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

    // ���ο� ��Ʈ�� �߰��ϰ� ����Ʈ ����
    public static MyPageEntryList LoadAll()
    {
        if (!File.Exists(path))
        {
            return new MyPageEntryList();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<MyPageEntryList>(json);
    }

    // ���� ���� ���� ���� (�ʱ�ȭ)
    public static void ClearAll()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
