using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 플랫포머 게임 매니저
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

    // 오디오
    public AudioSource clear;
    public AudioSource over;
    public AudioSource getApple;

    // 싱글톤 인스턴스
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

        // 타이머
        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            GameOver();
        }

        if (score >= 5) // 짝수 5개 이상 모으면
        {
            GameClear();
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

    // 점수 처리
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

    // 홀수 사과와 닿았을 때
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

    // 게임 오버
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

    // 게임 클리어
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

    // 몇 초 후 VN 파트 로드
    IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("VNPart");
    }

    // 자동으로 게임 재시작
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
