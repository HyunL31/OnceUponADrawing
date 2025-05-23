using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Flower �̴ϰ��� ��ũ��Ʈ
/// </summary>

public class FlowerQuizManager : MonoBehaviour
{
    // ���� ���� ����ü
    [System.Serializable]
    public class MathQuestion
    {
        public int operand1;
        public int operand2;
        public bool isAddition;

        // ���� ���
        public int Answer => isAddition ? operand1 + operand2 : operand1 - operand2;
    }

    public StartManager startManager;

    // UI ���
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;

    public GameObject successPanel;
    public GameObject failPanel;
    public GameObject winPanel;

    public Button[] flowerButtons;
    public Button checkButton;

    // �����
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

    // ���� ����
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
                op2 = Random.Range(1, op1);      // ������ �׻� ���
            }

            questions[i] = new MathQuestion
            {
                operand1 = op1,
                operand2 = op2,
                isAddition = useAddition
            };
        }
    }

    // ���� UI ǥ��
    void LoadQuestion()
    {
        clickCount = 0;
        timeLeft = 5f;
        isPlaying = true;

        var q = questions[currentQuestionIndex];
        string op = q.isAddition ? "+" : "-";

        questionText.text = $"{q.operand1} {op} {q.operand2}";
        timerText.text = "5��";

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
            timerText.text = Mathf.Ceil(timeLeft).ToString() + "��";
        }

        // Timeout
        if (timeLeft <= 0)
        {
            HandleFail();
        }
    }

    // Flower ��ư ���� ��
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

    // ���� ����
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

    // ������ ��
    void HandleSuccess()
    {
        isPlaying = false;
        success.Play();
        successPanel.SetActive(true);

        Invoke(nameof(NextQuestion), 2f);
    }

    // Ʋ�� ��
    void HandleFail()
    {
        isPlaying = false;
        fail.Play();
        failPanel.SetActive(true);

        Invoke(nameof(RetryQuestion), 2f);
    }

    // ���� ���� �ٽ�
    void RetryQuestion()
    {
        LoadQuestion();
    }

    // ���� ����
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