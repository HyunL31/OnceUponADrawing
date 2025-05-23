using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �÷����� ���� �Ŵ���
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlatformerManager platformerManager;

    // Score
    private int score = 0;
    public TextMeshProUGUI scoreText;

    // Game state
    private bool isGameOver = false;
    public float fallLimit = -5f;
    public int life = 3;
    public float gameTime = 40f;

    // UI
    public GameObject gameOver;
    public GameObject gameClear;
    public GameObject player;

    // �����
    public AudioSource clear;
    public AudioSource over;
    public AudioSource getApple;

    // �̱��� �ν��Ͻ�
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (IsGameOver())
        {
            return;
        }

        // Ÿ�̸�
        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            GameOver();
        }

        if (score >= 5) // ¦�� 5�� �̻� ������
        {
            GameClear();
        }

        // ĳ���Ͱ� �������� ���� ����
        if (player != null && player.transform.position.y < fallLimit)
        {
            GameOver();
        }
    }

    // ĳ����, ������ �̵� ���߱�
    void StopAllMoveLeft()
    {
        MoveLeft[] moveObjects = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);

        foreach (MoveLeft obj in moveObjects)
        {
            obj.Stop();
        }
    }

    // ���� ó��
    public void AddScore(int value)
    {
        if (IsGameOver())
        {
            return;
        }

        score += value;
        getApple.Play();

        string toString = score.ToString();
        scoreText.text = "Score: " + toString;
    }

    // Ȧ�� ����� ����� ��
    public void HitOddItem()
    {
        if (IsGameOver())
        {
            return;
        }

        life--;

        if (life <= 0)
        {
            GameOver();
        }
    }

    // ���� ����
    public void GameOver()
    {
        if (IsGameOver())
        {
            return;
        }

        isGameOver = true;

        StopAllMoveLeft();

        if (platformerManager.timerText != null)
        {
            Destroy(platformerManager.timerText.gameObject);
        }

        foreach (var heart in platformerManager.hearts)
        {
            if (heart != null)
            {
                Destroy(heart.gameObject);
            }
        }

        over.Play();
        gameOver.SetActive(true);
    }

    // ���� Ŭ����
    public void GameClear()
    {
        if (IsGameOver())
        {
            return;
        }

        isGameOver = true;

        StopAllMoveLeft();

        if (platformerManager.timerText != null)
        {
            Destroy(platformerManager.timerText.gameObject);
        }

        foreach (var heart in platformerManager.hearts)
        {
            if (heart != null)
            {
                Destroy(heart.gameObject);
            }
        }

        clear.Play();
        gameClear.SetActive(true);

        StartCoroutine(DelayedSceneLoad());
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public int GetScore()
    {
        return score;
    }

    // �� �� �� VN ��Ʈ �ε�
    IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("VNPart");
    }

    // �ڵ����� ���� �����
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
