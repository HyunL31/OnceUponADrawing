using System.Collections;
using UnityEngine;

/// <summary>
/// �̴ϰ��� ���� ��ũ��Ʈ
/// </summary>

public class StartManager : MonoBehaviour
{
    public GameObject startPanel;

    public bool isTutorial = true;
    public bool isStart = false;

    void Start()
    {
        if (!isTutorial)
        {
            startPanel.SetActive(true);

            StartCoroutine(HidePanelAfterDelay(1.5f));
        }
    }

    public void OnTutorialClosed()
    {
        startPanel.SetActive(true);
        StartCoroutine(HidePanelAfterDelay(1.5f));
    }

    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        startPanel.SetActive(false);
        isStart = true;
    }
}
