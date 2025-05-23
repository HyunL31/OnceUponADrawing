using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �÷����� ������, Ÿ�̸� ��Ʈ��
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

    // ������ ������Ʈ
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

    // Ÿ�̸�
    void UpdateTimer()
    {
        float timeLeft = Mathf.Max(GameManager.Instance.gameTime, 0f);
        timerText.text = $"{Mathf.CeilToInt(timeLeft)}";
    }
}
