using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShapeQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class ShapeQuestion
    {
        public int correctIndex; // 정답 버튼 인덱스
    }

    public Button[] shapeButtons; // 정답 선택 버튼들
    public GameObject[] shapeImageObjects; // 문제 이미지 (각 문제별로 하나씩)

    public GameObject correctFeedback;
    public GameObject wrongFeedback;

    public ShapeQuestion[] questions;
    private int currentQuestionIndex = 0;

    void Start()
    {
        LoadQuestion();
    }

    public void OnShapeClicked(int index)
    {
        if (index == questions[currentQuestionIndex].correctIndex)
        {
            correctFeedback.SetActive(true);
            Invoke(nameof(NextQuestion), 1.5f);
        }
        else
        {
            wrongFeedback.SetActive(true);
            Invoke(nameof(HideWrong), 1f);
        }
    }

    void LoadQuestion()
    {
        // 모든 문제 이미지 숨기기
        foreach (var obj in shapeImageObjects)
        {
            obj.SetActive(false);
        }

        if (currentQuestionIndex >= questions.Length)
        {
            SceneManager.LoadScene("VNPart");
        }

        // 현재 문제 이미지만 보여주기
        shapeImageObjects[currentQuestionIndex].SetActive(true);

        correctFeedback.SetActive(false);
        wrongFeedback.SetActive(false);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        LoadQuestion();
    }

    void HideWrong()
    {
        wrongFeedback.SetActive(false);
    }
}
