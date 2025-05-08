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

    [Header("결과 UI")]
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
        if (isAnswered) return; // 판정 끝났으면 Update 멈춤

        timer -= Time.deltaTime;
        timerText.text = $"{Mathf.CeilToInt(Mathf.Max(timer, 0))} 초";

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
                    Debug.Log($"씨앗 그리기 감지! 현재 개수: {seedDrawn}");
                    seedNum.text = $"씨앗 개수: {seedDrawn}";
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
        Debug.Log($"[채점] 씨앗 개수: {seedDrawn}, 정답: {correctAnswer}");

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
        Debug.Log("▶ 정답 처리 코루틴 시작");
        yield return new WaitForSeconds(delay);
        Debug.Log("▶ 다음 문제로 이동");
        OnConfirmNextQuestion();
    }

    public void OnRetryButtonPressed()
    {
        retryButton.gameObject.SetActive(false);
        failImage.SetActive(false);
        StartNextQuestion(); // 같은 문제 다시
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

        Debug.Log($"문제 생성: {questionText.text}, 정답: {correctAnswer}");
    }

    void ShowComplete()
    {
        timerText.text = "0";
        successImage.SetActive(false);
        fullSuccess.SetActive(true);
        Debug.Log("미니게임 전체 완료!");

        SceneManager.LoadScene("VNPart");
    }
}
