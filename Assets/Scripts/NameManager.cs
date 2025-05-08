using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Name Scene
/// </summary>

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
