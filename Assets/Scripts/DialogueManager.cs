using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 비주얼 노벨 시스템 스크립트
public class DialogueManager : MonoBehaviour
{
    public Image backgroundImage;
    public List<BackgroundData> backgroundList;

    public Image characterImage;
    public List<CharacterData> characterList;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    // 타이핑 효과
    [SerializeField]
    private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public AudioSource typingSound;

    // 캐릭터
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Sprite sprite;
    }

    // 배경
    [System.Serializable]
    public class BackgroundData
    {
        public string name;
        public Sprite sprite;
    }

    // JSON
    [System.Serializable]
    public class DialogueEntry
    {
        public int id;
        public string speaker;
        public string text;
        public string background;
        public int nextID;
        public string command;
        public string game;
        public Choice[] choices;
    }

    [System.Serializable]
    public class DialogueList
    {
        public DialogueEntry[] entries;
    }

    private List<DialogueEntry> dialogueData;
    private int currentID = 0;

    // 선택지
    [System.Serializable]
    public class Choice
    {
        public string text;
        public int nextID;
    }

    public GameObject leftButtonObj;
    public GameObject rightButtonObj;
    public GameObject leftButtonObjT;
    public GameObject rightButtonObjT;

    public Button leftButton;
    public TextMeshProUGUI leftButtonText;
    public Button rightButton;
    public TextMeshProUGUI rightButtonText;

    private int resumeID = -1;

    void Start()
    {
        LoadDialogue();
        ShowEntry(currentID);

        // 플레이어가 그린 캐릭터 추가
        Sprite playerSprite = DrawingLoader.LoadPlayerDrawing();

        if (playerSprite != null)
        {
            characterList.Add(new CharacterData { name = "빨간 망토", sprite = playerSprite });
        }
    }

    void Update()
    {
        // UI 클릭 감지 방지 (선택지 버튼 누를 때)
        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        //    return;

        if (Input.GetMouseButtonDown(0))  // 왼쪽 클릭 또는 터치
        {
            NextButton();
        }
    }

    private void LoadDialogue()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Dialogue/VN");
        string jsonString = jsonText.text;

        DialogueList data = JsonUtility.FromJson<DialogueList>(jsonString);
        dialogueData = new List<DialogueEntry>(data.entries);
    }

    private void ShowEntry(int id)
    {
        DialogueEntry entry = null;

        for (int i = 0; i < dialogueData.Count; i++)
        {
            if (dialogueData[i].id == id)
            {
                entry = dialogueData[i];

                break;
            }
        }

        // 미니게임
        if (!string.IsNullOrEmpty(entry.command))
        {
            if (entry.command == "start_minigame1")
            {
                SceneManager.LoadScene("Platformer");
            }
            else if (entry.command == "start_minigame2")
            {
                SceneManager.LoadScene("Flower");
            }
            else if (entry.command == "start_minigame3")
            {
                SceneManager.LoadScene("Wally");
            }
            else if (entry.command == "start_minigame4")
            {
                SceneManager.LoadScene("Fruit");
            }
            else if (entry.command == "start_minigame5")
            {
                SceneManager.LoadScene("Maze");
            }

            return;
        }

        // 대사
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(entry));

        // 배경
        if (!string.IsNullOrEmpty(entry.background))
        {
            Sprite bgSprite = GetBackgroundSprite(entry.background);

            if (bgSprite != null)
            {
                backgroundImage.sprite = bgSprite;
            }
        }

        // 캐릭터
        speakerText.text = entry.speaker;

        if (!string.IsNullOrEmpty(entry.speaker))
        {
            Sprite charSprite = GetCharacterSprite(entry.speaker);

            if (charSprite != null)
            {
                characterImage.sprite = charSprite;
                characterImage.gameObject.SetActive(true);
            }
            else
            {
                characterImage.gameObject.SetActive(false);
            }
        }
    }

    // 대사 출력
    private IEnumerator TypeText(DialogueEntry entry)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingSound != null)
        {
            typingSound.Play();
        }

        // 대사 한 글자씩 출력 (타이핑 효과)
        foreach (char c in entry.text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (typingSound != null)
        {
            typingSound.Pause();
        }

        // 다음 entry가 선택지일 경우
        if (entry.choices != null || entry.choices.Length > 0)
        {
            ShowChoices(entry.choices);
        }
    }

    // 선택지
    private void ShowChoices(Choice[] choices)
    {
        DialogueEntry entry = dialogueData.Find(e => e.id == currentID); // 현재 대사

        // 선택지 패널 및 버튼 활성화
        leftButtonObj.SetActive(true);
        rightButtonObj.SetActive(true);
        leftButtonObjT.SetActive(true);
        rightButtonObjT.SetActive(true);

        // 왼쪽 버튼 설정
        leftButtonText.text = choices.Length > 0 ? choices[0].text : "";
        leftButton.onClick.RemoveAllListeners();

        if (choices.Length > 0)
        {
            int nextID = choices[0].nextID;
            leftButton.onClick.AddListener(() =>
            {
                HideChoicePanel();
                currentID = nextID;
                ShowEntry(currentID);
            });
        }

        // 오른쪽 버튼 설정
        rightButtonText.text = choices.Length > 1 ? choices[1].text : "";
        rightButton.onClick.RemoveAllListeners();

        if (choices.Length > 1)
        {
            int nextID = choices[1].nextID;
            rightButton.onClick.AddListener(() =>
            {
                HideChoicePanel();
                currentID = nextID;
                ShowEntry(currentID);
            });
        }
    }

    private Sprite GetBackgroundSprite(string bgName)
    {
        foreach (BackgroundData bg in backgroundList)
        {
            if (bg.name == bgName)
            {
                return bg.sprite;
            }
        }
        return null;
    }

    private Sprite GetCharacterSprite(string characterName)
    {
        foreach (CharacterData data in characterList)
        {
            if (data.name == characterName)
            {
                return data.sprite;
            }
        }

        return null;
    }

    private void HideChoicePanel()
    {
        leftButtonObj.SetActive(false);
        rightButtonObj.SetActive(false);
        leftButtonObjT.SetActive(false);
        rightButtonObjT.SetActive(false);
    }

    // 계속 버튼
    public void NextButton()
    {
        DialogueEntry entry = dialogueData.Find(e => e.id == currentID);

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = entry.text;
            isTyping = false;

            if (typingSound != null)
            {
                typingSound.Pause();
            }

            // 선택지 있으면 바로 보여줌
            if (entry.choices != null && entry.choices.Length > 0)
            {
                ShowChoices(entry.choices);
            }

            return;
        }

        // 선택지가 없다면 nextID로 이동
        if (entry.choices == null || entry.choices.Length == 0)
        {
            currentID = entry.nextID;
            ShowEntry(currentID);
        }
    }

    public void SetResumeID(int id)
    {
        resumeID = id;
    }

    public void ResumeFromMinigame()
    {
        if (resumeID >= 0)
        {
            ShowEntry(resumeID);
            resumeID = -1;
        }
    }
}