using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene Control
/// </summary>

public class SceneController : MonoBehaviour
{
    public void TaleSelecting ()
    {
        SceneManager.LoadScene("TaleSelecting");
    }

    public void Naming()
    {
        SceneManager.LoadScene("Naming");
    }

    public void Part1()
    {
        BGMManager bgmManager = FindAnyObjectByType<BGMManager>();

        if (bgmManager != null)
        {
            Destroy(bgmManager.gameObject);
        }

        SceneManager.LoadScene("VNPart");
    }
}
