using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShapeQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class ShapeQuestion
    {
        public int correctIndex; // ���� ��ư �ε���
    }

    public Button[] shapeButtons; // ���� ���� ��ư��
    public GameObject[] shapeImageObjects; // ���� �̹��� (�� �������� �ϳ���)

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
        // ��� ���� �̹��� �����
        foreach (var obj in shapeImageObjects)
        {
            obj.SetActive(false);
        }

        if (currentQuestionIndex >= questions.Length)
        {
            SceneManager.LoadScene("VNPart");
        }

        // ���� ���� �̹����� �����ֱ�
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
