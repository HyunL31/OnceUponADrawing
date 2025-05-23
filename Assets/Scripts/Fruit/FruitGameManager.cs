using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Fruit 미니게임 스크립트
/// </summary>

public class FruitGameManager : MonoBehaviour
{
    public StartManager startManager;
    
    // UI 요소
    public TextMeshProUGUI redHPText;
    public TextMeshProUGUI wolfHPText;
    public Image redHPBar;
    public Image wolfHPBar;

    public TextMeshProUGUI number1Text;
    public TextMeshProUGUI number2Text;
    public GameObject[] operatorButtons;

    public GameObject winPanel;
    public GameObject losePanel;

    // 타이머
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

    // 오디오
    public AudioSource explosion;
    public AudioSource win;

    // 체력 변수
    public int maxHP = 10;
    public int redHP = 10;
    public int wolfHP = 10;

    // 문제 변수
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
            timerText.text = $"{Mathf.Ceil(currentTime)}초";
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

    // 결과 표기
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

    // 다음 문제
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

    // 정답 판정
    bool CheckAnswer(string op)
    {
        return (op == "<" && num1 < num2) || (op == ">" && num1 > num2) || (op == "=" && num1 == num2);
    }

    // 체력 업데이트
    void UpdateHP()
    {
        redHPBar.fillAmount = (float)redHP / maxHP;
        wolfHPBar.fillAmount = (float)wolfHP / maxHP;

        redHPText.text = redHP.ToString();
        wolfHPText.text = wolfHP.ToString();
    }

    // 승리 처리
    void ShowWin()
    {
        win.Play();
        winPanel.SetActive(true);

        SceneManager.LoadScene("VNPart");
    }

    // 실패 처리
    void ShowLose()
    {
        losePanel.SetActive(true);
        StartCoroutine(RestartAfterDelay());
    }

    // 몇 초 후 다시 시작
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 빨간 망토 공격 애니메이션
    void PlayRedAttack()
    {
        if (redFruitPrefab != null && redSpawnPoint != null && wolfTargetPoint != null)
        {
            GameObject fruit = Instantiate(redFruitPrefab, canvasParent);
            fruit.transform.position = redSpawnPoint.position;

            StartCoroutine(MoveFruit(fruit, wolfTargetPoint.position));
        }
    }

    // 늑대 공격 애니메이션
    void PlayWolfAttack()
    {
        if (wolfFruitPrefab != null && wolfAttackPoint != null && redTargetPoint != null)
        {
            GameObject projectile = Instantiate(wolfFruitPrefab, canvasParent);
            wolfFruitPrefab.transform.position = wolfAttackPoint.position;

            StartCoroutine(MoveFruit(projectile, redTargetPoint.position));
        }
    }

    // 사과 움직임
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
