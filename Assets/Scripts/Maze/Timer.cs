using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeTimer : MonoBehaviour
{
    public float timeLimit = 15f;
    private float currentTime;
    public TextMeshProUGUI timerText;
    public GrandmaFollow grandma;
    public GameObject failPanel;

    private bool running = false;
    private bool gameOverTriggered = false;

    void Start()
    {
        currentTime = timeLimit;
        timerText.text = timeLimit.ToString("F0");
        running = true;
    }

    void Update()
    {
        if (!running || gameOverTriggered) return;

        currentTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0f)
        {
            running = false;
            grandma.StopFollowing();
            GameOver();
        }
    }

    void GameOver()
    {
        if (gameOverTriggered) return;

        gameOverTriggered = true;
        failPanel.SetActive(true);
        Invoke(nameof(ReloadScene), 1f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("Maze");
    }
}