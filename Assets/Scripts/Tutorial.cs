using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 튜토리얼 패널 관리 스크립트
/// </summary>

public class TutorialManager : MonoBehaviour
{
    public StartManager startManager;

    public GameObject tutorialPanel;   // 튜토리얼 UI 패널
    public Button closeButton;         // 닫기 버튼

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
