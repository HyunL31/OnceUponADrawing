using UnityEngine;
using TMPro;

/// <summary>
/// 플레이어 이름 설정 시스템
/// </summary>

public class NameManager : MonoBehaviour
{
    // 이름
    public TMP_InputField nameInput;
    private string playerName = null;

    // 성격
    public GameObject personalityPanel;

    private void Start()
    {
        personalityPanel.SetActive(false);
    }

    // 플레이어 이름 받아서 저장
    public void CheckName()
    {
        playerName = nameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            SaveName();
        }
    }

    // 이름 저장
    public void SaveName()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        personalityPanel.SetActive(true);
    }

    // 클릭하면 성격과 동화 함께 저장
    public void OnClickSelectPersonality(string personality)
    {
        PlayerPrefs.SetString("Personality", personality);

        string currentStoryCode = PlayerPrefs.GetString("CurrentStoryCode", "RedHood");

        PlayerPrefs.SetString($"StoryPersonality{currentStoryCode}", personality);
    }
}