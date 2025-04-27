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
        int life = Mathf.Clamp(GameManager.Instance.life, 0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < life;
        }
    }

    void UpdateTimer()
    {
        float timeLeft = Mathf.Max(GameManager.Instance.gameTime, 0f);
        timerText.text = $"{Mathf.CeilToInt(timeLeft)}";
    }
}
