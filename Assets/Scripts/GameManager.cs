using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlatformerManager platformerManager;

    private int score = 0;
    private bool isGameOver = false;
    public float fallLimit = -5f;

    public int life = 3;
    public float gameTime = 40f;

    public GameObject gameOver;
    public GameObject gameClear;
    public GameObject player;


    // �Ŵ��� �ߺ� ����
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

        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        { 
            if (score >= 5) // ¦�� 5�� �̻� ������
            {
                GameClear();
            }
            else
            {
                GameOver();
            }
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

    public void AddScore(int value)
    {
        if (IsGameOver())
        {
            return;
        }

        score += value;

        Debug.Log("Score: " + score);
    }

    public void HitOddItem()
    {
        if (IsGameOver())
        {
            return;
        }

        life--;
        Debug.Log($"���� ��Ʈ: {life}");

        if (life <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (IsGameOver()) return;

        isGameOver = true;

        StopAllMoveLeft();

        if (platformerManager.timerText != null)
        {
            Destroy(platformerManager.timerText.gameObject);
        }

        foreach (var heart in platformerManager.hearts)
        {
            if (heart != null)
                Destroy(heart.gameObject);
        }

        gameOver.SetActive(true);
    }


    public void GameClear()
    {
        if (IsGameOver()) return;

        isGameOver = true;

        StopAllMoveLeft();

        if (platformerManager.timerText != null)
        {
            Destroy(platformerManager.timerText.gameObject);
        }

        foreach (var heart in platformerManager.hearts)
        {
            if (heart != null)
                Destroy(heart.gameObject);
        }

        gameClear.SetActive(true);

        // ���� ������ �� ���־� �뺧 ��Ʈ�� �̵�
        DialogueManager vnManager = FindObjectsByType<DialogueManager>();

        if (vnManager != null)
        {
            vnManager.ResumeFromMinigame();
        }
    }

    private T FindObjectsByType<T>()
    {
        throw new NotImplementedException();
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public int GetScore()
    {
        return score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
