using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ��Ʈ, Ÿ�̸� ���� ��ũ��Ʈ
public class PlatformerManager : MonoBehaviour
{
    public Image[] hearts;
    public TextMeshProUGUI timerText;

    void Update()
    {
        UpdateHearts();
        UpdateTimer();
    }

    void UpdateHearts()
    {
        if (hearts == null) return;

        int life = Mathf.Clamp(GameManager.Instance.life, 0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].enabled = i < life;
            }
        }
    }

    void UpdateTimer()
    {
        float timeLeft = Mathf.Max(GameManager.Instance.gameTime, 0f);
        timerText.text = $"{Mathf.CeilToInt(timeLeft)}";
    }
}
