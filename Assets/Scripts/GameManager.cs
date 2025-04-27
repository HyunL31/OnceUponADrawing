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


    // 매니저 중복 방지
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
            if (score >= 5) // 짝수 5개 이상 모으면
            {
                GameClear();
            }
            else
            {
                GameOver();
            }
        }

        // 캐릭터가 떨어지면 게임 오버
        if (player != null && player.transform.position.y < fallLimit)
        {
            GameOver();
        }
    }

    // 캐릭터, 아이템 이동 멈추기
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
        Debug.Log($"남은 하트: {life}");

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

        // 게임 마무리 후 비주얼 노벨 파트로 이동
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
