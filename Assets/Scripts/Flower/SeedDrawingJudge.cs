using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SeedDrawingJudge : MonoBehaviour
{
    public int totalQuestions = 4;
    private int currentQuestion = 0;

    public int correctAnswer = 0;
    private int seedDrawn = 0;

    public float maxTime = 10f;
    private float timer;

    private Vector2 startPos;
    private bool isTouching = false;

    private bool isAnswered = false;

    [Header("UI")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI seedNum;
    public Button nextButton;

    [Header("��� UI")]
    public GameObject gameStart;
    public GameObject fullSuccess;
    public GameObject successImage;
    public GameObject failImage;
    public Button retryButton;

    IEnumerator Start()
    {
        gameStart.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gameStart.SetActive(false);

        yield return null;
        StartNextQuestion();
    }

    void Update()
    {
        if (isAnswered) return; // ���� �������� Update ����

        timer -= Time.deltaTime;
        timerText.text = $"{Mathf.CeilToInt(Mathf.Max(timer, 0))} ��";

        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                DrawingSystem.Instance.targetImage.rectTransform,
                Input.mousePosition, null, out startPos);
            isTouching = true;
        }

        if (Input.GetMouseButtonUp(0) && isTouching)
        {
            Vector2 endPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                DrawingSystem.Instance.targetImage.rectTransform,
                Input.mousePosition, null, out endPos);

            if (RectTransformUtility.RectangleContainsScreenPoint(
                DrawingSystem.Instance.targetImage.rectTransform,
                Input.mousePosition, null))
            {
                float distance = Vector2.Distance(startPos, endPos);

                if (distance >= 2f)
                {
                    seedDrawn++;
                    Debug.Log($"���� �׸��� ����! ���� ����: {seedDrawn}");
                    seedNum.text = $"���� ����: {seedDrawn}";
                }
            }

            isTouching = false;
        }
    }

    public void OnConfirmNextQuestion()
    {
        currentQuestion++;

        if (currentQuestion < totalQuestions)
        {
            StartNextQuestion();
        }
        else
        {
            ShowComplete();
        }
    }

    public void OnNextButtonPressed()
    {
        if (isAnswered) return;

        isAnswered = true;

        bool isCorrect = seedDrawn == correctAnswer;
        Debug.Log($"[ä��] ���� ����: {seedDrawn}, ����: {correctAnswer}");

        if (isCorrect)
        {
            successImage.SetActive(true);
            StartCoroutine(WaitAndGoNext(1.5f));
        }
        else
        {
            failImage.SetActive(true);
            retryButton.gameObject.SetActive(true);
        }
    }

    IEnumerator WaitAndGoNext(float delay)
    {
        Debug.Log("�� ���� ó�� �ڷ�ƾ ����");
        yield return new WaitForSeconds(delay);
        Debug.Log("�� ���� ������ �̵�");
        OnConfirmNextQuestion();
    }

    public void OnRetryButtonPressed()
    {
        retryButton.gameObject.SetActive(false);
        failImage.SetActive(false);
        StartNextQuestion(); // ���� ���� �ٽ�
    }

    void StartNextQuestion()
    {
        DrawingSystem.Instance.ClearScreen();

        seedDrawn = 0;
        timer = maxTime;
        isAnswered = false;

        GenerateQuestion();

        successImage.SetActive(false);
        failImage.SetActive(false);
    }

    void GenerateQuestion()
    {
        int a = Random.Range(2, 10);
        int b = Random.Range(2, 10);
        bool isPlus = Random.value < 0.5f;

        if (isPlus)
        {
            correctAnswer = a + b;
            questionText.text = $"{a} + {b} = ?";
        }
        else
        {
            if (a < b) (a, b) = (b, a);
            correctAnswer = a - b;
            questionText.text = $"{a} - {b} = ?";
        }

        Debug.Log($"���� ����: {questionText.text}, ����: {correctAnswer}");
    }

    void ShowComplete()
    {
        timerText.text = "0";
        successImage.SetActive(false);
        fullSuccess.SetActive(true);
        Debug.Log("�̴ϰ��� ��ü �Ϸ�!");

        SceneManager.LoadScene("VNPart");
    }
}
