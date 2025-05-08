using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public int totalQuestions = 10;

    public int maxHP = 10;
    public int redHP = 10;
    public int wolfHP = 10;

    public TextMeshProUGUI redHPText;
    public TextMeshProUGUI wolfHPText;

    public TextMeshProUGUI number1Text;
    public TextMeshProUGUI number2Text;
    public GameObject[] operatorButtons; // < > = 버튼들

    public Image redHPBar;
    public Image wolfHPBar;

    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI timerText;
    public float timeLimit = 3f;
    private float currentTime;
    private bool answered = false;

    private int a, b;

    void Start()
    {
        LoadNextQuestion();
    }

    void Update()
    {
        if (answered) return;

        currentTime -= Time.deltaTime;
        timerText.text = $"{Mathf.Ceil(currentTime)}";

        if (currentTime <= 0f)
        {
            answered = true;
            redHP--;
            UpdateHP();
            PlayWolfAttack();

            if (redHP <= 0)
                ShowLose();
            else
                Invoke(nameof(LoadNextQuestion), 1.2f);
        }
    }


    public void OnOperatorSelected(string op)
    {
        if (answered) return;
        answered = true;

        bool isCorrect = CheckAnswer(op);

        if (isCorrect)
        {
            wolfHP--;
            UpdateHP();
            PlayRedAttack();
        }
        else
        {
            redHP--;
            UpdateHP();
            PlayWolfAttack();
        }

        if (wolfHP <= 0)
            ShowWin();
        else if (redHP <= 0)
            ShowLose();
        else
            Invoke(nameof(LoadNextQuestion), 1.2f);
    }


    void LoadNextQuestion()
    {
        answered = false;
        currentTime = timeLimit;

        a = Random.Range(1, 20);
        b = Random.Range(1, 20);

        number1Text.text = a.ToString();
        number2Text.text = b.ToString();
        timerText.text = Mathf.Ceil(currentTime).ToString();
    }


    bool CheckAnswer(string op)
    {
        return (op == "<" && a < b) || (op == ">" && a > b) || (op == "=" && a == b);
    }
    void UpdateHP()
    {
        redHPBar.fillAmount = (float)redHP / maxHP;
        wolfHPBar.fillAmount = (float)wolfHP / maxHP;

        redHPText.text = redHP.ToString();
        wolfHPText.text = wolfHP.ToString();
    }

    void ShowWin()
    {
        winPanel.SetActive(true);

        SceneManager.LoadScene("VNPart");
    }

    void ShowLose()
    {
        losePanel.SetActive(true);
    }

    void PlayRedAttack() { /* 애니메이션 or 이펙트 */ }
    void PlayWolfAttack() { /* 애니메이션 or 이펙트 */ }
}
