using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 플랫포머 라이프, 타이머 컨트롤
/// </summary>

public class PlatformerManager : MonoBehaviour
{
    public StartManager startManager;

    public Image[] hearts;

    public TextMeshProUGUI timerText;

    void Update()
    {
        if (startManager.isStart)
        {
            UpdateHearts();
            UpdateTimer();
        }
    }

    // 라이프 업데이트
    void UpdateHearts()
    {
        if (hearts == null)
        {
            return;
        }

        int life = Mathf.Clamp(GameManager.Instance.life, 0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].enabled = i < life;
            }
        }
    }

    // 타이머
    void UpdateTimer()
    {
        float timeLeft = Mathf.Max(GameManager.Instance.gameTime, 0f);
        timerText.text = $"{Mathf.CeilToInt(timeLeft)}";
    }
}
