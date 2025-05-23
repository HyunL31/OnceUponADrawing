using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Visual Novel Script
/// </summary>

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private int currentStoryID = 1;

    // 스토리 분기 구조체
    [System.Serializable]
    public class Branch
    {
        public string personality;
        public int nextID;
    }

    // UI 연결
    public Image backgroundImage;
    public List<BackgroundData> backgroundList;

    public Image characterImage;
    public List<CharacterData> characterList;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    // 대사 타이핑 효과
    [SerializeField]
    private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public AudioSource typingSound;

    // 캐릭터 데이터
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Sprite sprite;
    }

    // 배경 이미지
    [System.Serializable]
    public class BackgroundData
    {
        public string name;
        public Sprite sprite;
    }

    // 대사 항목 (Json 파일)
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
        public string music;
        public string sfx;
        public bool shake;      // 카메라
        public Branch[] branches;
    }

    // 대사 관리
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

    // 선택지 버튼 & 텍스트
    public GameObject leftButtonObj;
    public GameObject rightButtonObj;
    public GameObject leftButtonObjT;
    public GameObject rightButtonObjT;

    public Button leftButton;
    public TextMeshProUGUI leftButtonText;
    public Button rightButton;
    public TextMeshProUGUI rightButtonText;

    // 타이핑 완료 시 화살표
    public GameObject arrow;

    // 음악 데이터
    [System.Serializable]
    public class MusicData
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource musicSource;
    public List<MusicData> musicList;

    // 효과음 데이터
    [System.Serializable]
    public class SfxData
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource sfxSource;
    public List<SfxData> sfxList;

    // Json 파일 로드 & 초기 대사
    void Start()
    {
        LoadDialogue();

        // 미니 게임에서 복귀 후 이어서 시작
        if (PlayerPrefs.HasKey("ResumeID"))
        {
            currentID = PlayerPrefs.GetInt("ResumeID");
            PlayerPrefs.DeleteKey("ResumeID");
        }

        ShowEntry(currentID);

        // 플레이어 캐릭터 로드
        Sprite playerSprite = DrawingLoader.LoadPlayerDrawing();

        if (playerSprite != null)
        {
            characterList.Add(new CharacterData { name = "{playerName}", sprite = playerSprite });
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextButton();
        }

        if (isTyping)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
        }
    }

    // Json 파일 로드
    private void LoadDialogue()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Dialogue/RedHood");
        string jsonString = jsonText.text;

        DialogueList data = JsonUtility.FromJson<DialogueList>(jsonString);
        dialogueData = new List<DialogueEntry>(data.entries);
    }

    // 현재 대사 표시
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

        if (!string.IsNullOrEmpty(entry.command))
        {
            if (entry.command == "branch_by_personality")
            {
                string personality = PlayerPrefs.GetString("Personality", "Kind");      // 기본값 Kind

                foreach (var branch in entry.branches)
                {
                    if (branch.personality == personality)
                    {
                        currentID = branch.nextID;
                        ShowEntry(currentID);
                        return;
                    }
                }

                return;
            }

            // 미니 게임 전환 & 스토리 종료 시 저장
            if (entry.nextID >= 0)
            {
                PlayerPrefs.SetInt("ResumeID", entry.nextID);
            }

            // 미니게임
            switch (entry.command)
            {
                case "start_minigame1":
                    SceneManager.LoadScene("Platformer");
                    break;
                case "start_minigame2":
                    SceneManager.LoadScene("Flower");
                    break;
                case "start_minigame3":
                    SceneManager.LoadScene("Shape");
                    break;
                case "start_minigame4":
                    SceneManager.LoadScene("Fruit");
                    break;
                case "start_minigame5":
                    SceneManager.LoadScene("Maze");
                    break;
            }

            // 스토리 종료 시 (-1) 다음 스토리 해금
            if (entry.nextID == -1)
            {
                int nextStoryID = currentStoryID + 1;
                StoryUnlockManager.Instance.UnlockStory(nextStoryID);
                SceneManager.LoadScene("TaleSelecting");
            }

            return;
        }

        // 이름 치환
        if (!string.IsNullOrEmpty(entry.text))
        {
            string playerName = PlayerPrefs.GetString("PlayerName", "주인공"); // 기본값 설정
            entry.text = entry.text.Replace("{playerName}", playerName);
        }

        // 타이핑 시작
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(entry));

        // 배경 처리
        if (!string.IsNullOrEmpty(entry.background))
        {
            Sprite bgSprite = GetBackgroundSprite(entry.background);

            if (bgSprite != null)
            {
                backgroundImage.sprite = bgSprite;
            }
        }

        // 음악 처리
        if (!string.IsNullOrEmpty(entry.music))
        {
            AudioClip musicClip = GetMusicClip(entry.music);
            if (musicClip != null && musicSource.clip != musicClip)
            {
                musicSource.clip = musicClip;
                musicSource.Play();
            }
        }

        // SFX 처리
        if (!string.IsNullOrEmpty(entry.sfx))
        {
            AudioClip sfxClip = GetSfxClip(entry.sfx);
            if (sfxClip != null)
            {
                sfxSource.PlayOneShot(sfxClip);
            }
        }

        // 대사하는 캐릭터 처리
        if (entry.speaker == "{playerName}")
        {
            speakerText.text = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            speakerText.text = entry.speaker;
        }

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

    private AudioClip GetMusicClip(string musicName)
    {
        foreach (var m in musicList)
        {
            if (m.name == musicName)
            {
                return m.clip;
            }
        }

        return null;
    }

    private AudioClip GetSfxClip(string sfxName)
    {
        foreach (var s in sfxList)
        {
            if (s.name == sfxName)
            {
                return s.clip;
            }
        }

        return null;
    }

    // 대사 타이핑
    private IEnumerator TypeText(DialogueEntry entry)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingSound != null)
        {
            typingSound.Play();
        }

        // 타이핑 효과
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

        // 선택지 처리
        if (entry.choices != null || entry.choices.Length > 0)
        {
            ShowChoices(entry.choices);
        }
    }

    // 선택지
    private void ShowChoices(Choice[] choices)
    {
        DialogueEntry entry = dialogueData.Find(e => e.id == currentID); // 현재 대사 (람다 함수)

        // 선택지 UI 활성화
        leftButtonObj.SetActive(true);
        rightButtonObj.SetActive(true);
        leftButtonObjT.SetActive(true);
        rightButtonObjT.SetActive(true);

        // 선택지 버튼 1
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

        // 선택지 버튼 2
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

    // 다음 대사 넘어가기
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

            // 선택지
            if (entry.choices != null && entry.choices.Length > 0)
            {
                ShowChoices(entry.choices);
            }

            if (entry.nextID == -1)
            {
                int nextStoryID = currentStoryID + 1;
                StoryUnlockManager.Instance.UnlockStory(nextStoryID);

                SceneManager.LoadScene("TaleSelecting");

                return;
            }

            return;
        }

        // 선택지 없을 시 다음 대사로 이동
        if (entry.choices == null || entry.choices.Length == 0)
        {
            currentID = entry.nextID;
            ShowEntry(currentID);
        }
    }
}