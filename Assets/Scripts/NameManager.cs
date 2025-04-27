using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// 이름 시스템 스크립트
public class NameManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    private string playerName = null;

    public void CheckName()
    {
        playerName = nameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            SaveName();
        }
    }

    public void SaveName()
    {
        PlayerPrefs.SetString("currentName", playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("TaleSelecting");
    }
}
