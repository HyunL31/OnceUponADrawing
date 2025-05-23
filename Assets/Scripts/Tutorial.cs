using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ʃ�丮�� �г� ���� ��ũ��Ʈ
/// </summary>

public class TutorialManager : MonoBehaviour
{
    public StartManager startManager;

    public GameObject tutorialPanel;   // Ʃ�丮�� UI �г�
    public Button closeButton;         // �ݱ� ��ư

    void Start()
    {
        tutorialPanel.SetActive(true);

        closeButton.onClick.AddListener(CloseTutorial);
    }

    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        startManager.isTutorial = false;
        startManager.OnTutorialClosed();
    }
}
