using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Shape 미니게임 스크립트
/// </summary>

public class ShapeQuizManager : MonoBehaviour
{
    // 정답 인덱스
    [System.Serializable]
    public class ShapeQuestion
    {
        public int correctIndex;
    }

    // 도형 설명 패널
    [System.Serializable]
    public class ExplanationGroup
    {
        public GameObject[] panels;
    }

    // UI 요소
    public Button[] shapeButtons;
    public GameObject[] shapeImageObjects;
    public GameObject correctFeedback;

    // 문제 데이터
    public ShapeQuestion[] questions;
    public ExplanationGroup[] allExplanations;

    // 오디오
    public AudioSource fail;
    public AudioSource success;

    private int currentQuestionIndex = 0;

    void Start()
    {
        // 모든 해설 패널 비활성화
        foreach (var group in allExplanations)
        {
            foreach (var panel in group.panels)
            {
                panel.SetActive(false);
            }
        }

        LoadQuestion();
    }

    // 정답 판정
    public void OnShapeClicked(int index)
    {
        if (index == questions[currentQuestionIndex].correctIndex)
        {
            correctFeedback.SetActive(true);
            success.Play();

            Invoke(nameof(NextQuestion), 1.5f);     // 몇 초 후 다음 문제 로드
        }
        else
        {
            // 정답이 아닌 경우에만 설명 패널 띄우기
            if (index < allExplanations[currentQuestionIndex].panels.Length &&
                allExplanations[currentQuestionIndex].panels[index] != null)
            {
                allExplanations[currentQuestionIndex].panels[index].SetActive(true);
                fail.Play();

            }
        }
    }

    // 도형 설명 확인 완료 시
    public void OnExplanationOK(int clickedIndex)
    {
        allExplanations[currentQuestionIndex].panels[clickedIndex].SetActive(false);
    }

    // 문제 로드 시
    void LoadQuestion()
    {
        foreach (var obj in shapeImageObjects)
        {
            obj.SetActive(false);
        }

        if (currentQuestionIndex >= questions.Length)
        {
            SceneManager.LoadScene("VNPart");

            return;
        }

        shapeImageObjects[currentQuestionIndex].SetActive(true);
        correctFeedback.SetActive(false);
    }

    // 다음 문제
    void NextQuestion()
    {
        currentQuestionIndex++;
        LoadQuestion();
    }
}
