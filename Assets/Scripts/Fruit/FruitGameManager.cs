using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Fruit �̴ϰ��� ��ũ��Ʈ
/// </summary>

public class FruitGameManager : MonoBehaviour
{
    public StartManager startManager;
    
    // UI ���
    public TextMeshProUGUI redHPText;
    public TextMeshProUGUI wolfHPText;
    public Image redHPBar;
    public Image wolfHPBar;

    public TextMeshProUGUI number1Text;
    public TextMeshProUGUI number2Text;
    public GameObject[] operatorButtons;

    public GameObject winPanel;
    public GameObject losePanel;

    // Ÿ�̸�
    public TextMeshProUGUI timerText;
    public float timeLimit = 3f;
    private float currentTime;
    private bool answered = false;

    [Header("Attack")]
    public GameObject redFruitPrefab;
    public GameObject wolfFruitPrefab;
    public Transform redSpawnPoint;
    public Transform wolfAttackPoint;
    public Transform redTargetPoint;
    public Transform wolfTargetPoint;

    public Transform canvasParent;

    // �����
    public AudioSource explosion;
    public AudioSource win;

    // ü�� ����
    public int maxHP = 10;
    public int redHP = 10;
    public int wolfHP = 10;

    // ���� ����
    private int num1, num2;

    void Start()
    {
        LoadNextQuestion();
    }

    void Update()
    {
        if (answered)
        {
            return;
        }
        
        if (startManager.isStart)
        {
            currentTime -= Time.deltaTime;
            timerText.text = $"{Mathf.Ceil(currentTime)}��";
        }

        // Timeout
        if (currentTime <= 0f)
        {
            answered = true;
            redHP--;
            UpdateHP();
            PlayWolfAttack();

            if (redHP <= 0)
            {
                ShowLose();
            }
            else
            {
                Invoke(nameof(LoadNextQuestion), 1.2f);
            }
        }
    }

    // ��� ǥ��
    public void OnOperatorSelected(string op)
    {
        if (answered)
        {
            return;
        }

        answered = true;

        bool isCorrect = CheckAnswer(op);

        if (isCorrect)
        {
            wolfHP--;
            UpdateHP();
            PlayRedAttack();
        }
        else
        {
            redHP--;
            UpdateHP();
            PlayWolfAttack();
        }

        if (wolfHP <= 0)
        {
            ShowWin();
        }
        else if (redHP <= 0)
        {
            ShowLose();
        }
        else
        {
            Invoke(nameof(LoadNextQuestion), 1.2f);
        }
    }

    // ���� ����
    void LoadNextQuestion()
    {
        answered = false;
        currentTime = timeLimit;

        num1 = Random.Range(1, 20);
        num2 = Random.Range(1, 20);

        number1Text.text = num1.ToString();
        number2Text.text = num2.ToString();
        timerText.text = Mathf.Ceil(currentTime).ToString();
    }

    // ���� ����
    bool CheckAnswer(string op)
    {
        return (op == "<" && num1 < num2) || (op == ">" && num1 > num2) || (op == "=" && num1 == num2);
    }

    // ü�� ������Ʈ
    void UpdateHP()
    {
        redHPBar.fillAmount = (float)redHP / maxHP;
        wolfHPBar.fillAmount = (float)wolfHP / maxHP;

        redHPText.text = redHP.ToString();
        wolfHPText.text = wolfHP.ToString();
    }

    // �¸� ó��
    void ShowWin()
    {
        win.Play();
        winPanel.SetActive(true);

        SceneManager.LoadScene("VNPart");
    }

    // ���� ó��
    void ShowLose()
    {
        losePanel.SetActive(true);
        StartCoroutine(RestartAfterDelay());
    }

    // �� �� �� �ٽ� ����
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ���� ���� ���� �ִϸ��̼�
    void PlayRedAttack()
    {
        if (redFruitPrefab != null && redSpawnPoint != null && wolfTargetPoint != null)
        {
            GameObject fruit = Instantiate(redFruitPrefab, canvasParent);
            fruit.transform.position = redSpawnPoint.position;

            StartCoroutine(MoveFruit(fruit, wolfTargetPoint.position));
        }
    }

    // ���� ���� �ִϸ��̼�
    void PlayWolfAttack()
    {
        if (wolfFruitPrefab != null && wolfAttackPoint != null && redTargetPoint != null)
        {
            GameObject projectile = Instantiate(wolfFruitPrefab, canvasParent);
            wolfFruitPrefab.transform.position = wolfAttackPoint.position;

            StartCoroutine(MoveFruit(projectile, redTargetPoint.position));
        }
    }

    // ��� ������
    IEnumerator MoveFruit(GameObject obj, Vector3 target)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 start = obj.transform.position;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (CameraShake.Instance != null)
        {
            StartCoroutine(CameraShake.Instance.ShakeCamera(0.2f, 0.15f));
        }

        explosion.Play();

        Destroy(obj);
    }
}
