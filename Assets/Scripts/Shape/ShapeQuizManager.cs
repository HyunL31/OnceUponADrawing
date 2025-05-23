using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Shape �̴ϰ��� ��ũ��Ʈ
/// </summary>

public class ShapeQuizManager : MonoBehaviour
{
    // ���� �ε���
    [System.Serializable]
    public class ShapeQuestion
    {
        public int correctIndex;
    }

    // ���� ���� �г�
    [System.Serializable]
    public class ExplanationGroup
    {
        public GameObject[] panels;
    }

    // UI ���
    public Button[] shapeButtons;
    public GameObject[] shapeImageObjects;
    public GameObject correctFeedback;

    // ���� ������
    public ShapeQuestion[] questions;
    public ExplanationGroup[] allExplanations;

    // �����
    public AudioSource fail;
    public AudioSource success;

    private int currentQuestionIndex = 0;

    void Start()
    {
        // ��� �ؼ� �г� ��Ȱ��ȭ
        foreach (var group in allExplanations)
        {
            foreach (var panel in group.panels)
            {
                panel.SetActive(false);
            }
        }

        LoadQuestion();
    }

    // ���� ����
    public void OnShapeClicked(int index)
    {
        if (index == questions[currentQuestionIndex].correctIndex)
        {
            correctFeedback.SetActive(true);
            success.Play();

            Invoke(nameof(NextQuestion), 1.5f);     // �� �� �� ���� ���� �ε�
        }
        else
        {
            // ������ �ƴ� ��쿡�� ���� �г� ����
            if (index < allExplanations[currentQuestionIndex].panels.Length &&
                allExplanations[currentQuestionIndex].panels[index] != null)
            {
                allExplanations[currentQuestionIndex].panels[index].SetActive(true);
                fail.Play();

            }
        }
    }

    // ���� ���� Ȯ�� �Ϸ� ��
    public void OnExplanationOK(int clickedIndex)
    {
        allExplanations[currentQuestionIndex].panels[clickedIndex].SetActive(false);
    }

    // ���� �ε� ��
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

    // ���� ����
    void NextQuestion()
    {
        currentQuestionIndex++;
        LoadQuestion();
    }
}
