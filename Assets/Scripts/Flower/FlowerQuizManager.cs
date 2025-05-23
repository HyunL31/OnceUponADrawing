using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Flower 미니게임 스크립트
/// </summary>

public class FlowerQuizManager : MonoBehaviour
{
    // 수학 문제 구조체
    [System.Serializable]
    public class MathQuestion
    {
        public int operand1;
        public int operand2;
        public bool isAddition;

        // 정답 계산
        public int Answer => isAddition ? operand1 + operand2 : operand1 - operand2;
    }

    public StartManager startManager;

    // UI 요소
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;

    public GameObject successPanel;
    public GameObject failPanel;
    public GameObject winPanel;

    public Button[] flowerButtons;
    public Button checkButton;

    // 오디오
    public AudioSource button;
    public AudioSource fail;
    public AudioSource success;

    private int currentQuestionIndex = 0;
    private int clickCount = 0;
    private float timeLeft;
    private bool isPlaying = false;

    private MathQuestion[] questions;

    void Start()
    {
        GenerateQuestions();
        checkButton.onClick.AddListener(OnCheckAnswer);
        LoadQuestion();
    }

    // 문제 생성
    void GenerateQuestions()
    {
        questions = new MathQuestion[5];

        for (int i = 0; i < 5; i++)
        {
            bool useAddition = Random.value < 0.5f;

            int op1, op2;

            if (useAddition)
            {
                op1 = Random.Range(1, 6);
                op2 = Random.Range(1, 6);
            }
            else
            {
                op1 = Random.Range(2, 11);
                op2 = Random.Range(1, op1);      // 정답은 항상 양수
            }

            questions[i] = new MathQuestion
            {
                operand1 = op1,
                operand2 = op2,
                isAddition = useAddition
            };
        }
    }

    // 문제 UI 표기
    void LoadQuestion()
    {
        clickCount = 0;
        timeLeft = 5f;
        isPlaying = true;

        var q = questions[currentQuestionIndex];
        string op = q.isAddition ? "+" : "-";

        questionText.text = $"{q.operand1} {op} {q.operand2}";
        timerText.text = "5초";

        foreach (var btn in flowerButtons)
        {
            btn.gameObject.SetActive(true);
        }

        successPanel.SetActive(false);
        failPanel.SetActive(false);
        checkButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        // Timer
        if (startManager.isStart)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeLeft).ToString() + "초";
        }

        // Timeout
        if (timeLeft <= 0)
        {
            HandleFail();
        }
    }

    // Flower 버튼 누를 시
    public void OnFlowerClicked(Button clickedFlower)
    {
        if (!isPlaying)
        {
            return;
        }

        clickCount++;
        button.Play();
        clickedFlower.gameObject.SetActive(false);
    }

    // 정답 판정
    void OnCheckAnswer()
    {
        if (!isPlaying)
        {
            return;
        }

        if (clickCount == questions[currentQuestionIndex].Answer)
        {
            HandleSuccess();
        }
        else
        {
            HandleFail();
        }

        checkButton.gameObject.SetActive(false);
    }

    // 정답일 시
    void HandleSuccess()
    {
        isPlaying = false;
        success.Play();
        successPanel.SetActive(true);

        Invoke(nameof(NextQuestion), 2f);
    }

    // 틀릴 시
    void HandleFail()
    {
        isPlaying = false;
        fail.Play();
        failPanel.SetActive(true);

        Invoke(nameof(RetryQuestion), 2f);
    }

    // 같은 문제 다시
    void RetryQuestion()
    {
        LoadQuestion();
    }

    // 다음 문제
    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Length)
        {
            success.Play();
            winPanel.SetActive(true);

            SceneManager.LoadScene("VNPart");
        }
        else
        {
            LoadQuestion();
        }
    }
}