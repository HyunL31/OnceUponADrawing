using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene Control
/// </summary>

public class SceneController : MonoBehaviour
{
    public void Naming()
    {
        SceneManager.LoadScene("Naming");
    }

    public void Part1()
    {
        SceneManager.LoadScene("VNPart");
    }

    public void DrawRedRiding()
    {
        SceneManager.LoadScene("DrawingRedRiding");

        BGMManager bgmManager = FindAnyObjectByType<BGMManager>();

        Destroy(bgmManager.gameObject);
    }
}
