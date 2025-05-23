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

    // ���丮 �б� ����ü
    [System.Serializable]
    public class Branch
    {
        public string personality;
        public int nextID;
    }

    // UI ����
    public Image backgroundImage;
    public List<BackgroundData> backgroundList;

    public Image characterImage;
    public List<CharacterData> characterList;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    // ��� Ÿ���� ȿ��
    [SerializeField]
    private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public AudioSource typingSound;

    // ĳ���� ������
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Sprite sprite;
    }

    // ��� �̹���
    [System.Serializable]
    public class BackgroundData
    {
        public string name;
        public Sprite sprite;
    }

    // ��� �׸� (Json ����)
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
        public bool shake;      // ī�޶�
        public Branch[] branches;
    }

    // ��� ����
    [System.Serializable]
    public class DialogueList
    {
        public DialogueEntry[] entries;
    }

    private List<DialogueEntry> dialogueData;
    private int currentID = 0;

    // ������
    [System.Serializable]
    public class Choice
    {
        public string text;
        public int nextID;
    }

    // ������ ��ư & �ؽ�Ʈ
    public GameObject leftButtonObj;
    public GameObject rightButtonObj;
    public GameObject leftButtonObjT;
    public GameObject rightButtonObjT;

    public Button leftButton;
    public TextMeshProUGUI leftButtonText;
    public Button rightButton;
    public TextMeshProUGUI rightButtonText;

    // Ÿ���� �Ϸ� �� ȭ��ǥ
    public GameObject arrow;

    // ���� ������
    [System.Serializable]
    public class MusicData
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource musicSource;
    public List<MusicData> musicList;

    // ȿ���� ������
    [System.Serializable]
    public class SfxData
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource sfxSource;
    public List<SfxData> sfxList;

    // Json ���� �ε� & �ʱ� ���
    void Start()
    {
        LoadDialogue();

        // �̴� ���ӿ��� ���� �� �̾ ����
        if (PlayerPrefs.HasKey("ResumeID"))
        {
            currentID = PlayerPrefs.GetInt("ResumeID");
            PlayerPrefs.DeleteKey("ResumeID");
        }

        ShowEntry(currentID);

        // �÷��̾� ĳ���� �ε�
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

    // Json ���� �ε�
    private void LoadDialogue()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Dialogue/RedHood");
        string jsonString = jsonText.text;

        DialogueList data = JsonUtility.FromJson<DialogueList>(jsonString);
        dialogueData = new List<DialogueEntry>(data.entries);
    }

    // ���� ��� ǥ��
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
                string personality = PlayerPrefs.GetString("Personality", "Kind");      // �⺻�� Kind

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

            // �̴� ���� ��ȯ & ���丮 ���� �� ����
            if (entry.nextID >= 0)
            {
                PlayerPrefs.SetInt("ResumeID", entry.nextID);
            }

            // �̴ϰ���
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

            // ���丮 ���� �� (-1) ���� ���丮 �ر�
            if (entry.nextID == -1)
            {
                int nextStoryID = currentStoryID + 1;
                StoryUnlockManager.Instance.UnlockStory(nextStoryID);
                SceneManager.LoadScene("TaleSelecting");
            }

            return;
        }

        // �̸� ġȯ
        if (!string.IsNullOrEmpty(entry.text))
        {
            string playerName = PlayerPrefs.GetString("PlayerName", "���ΰ�"); // �⺻�� ����
            entry.text = entry.text.Replace("{playerName}", playerName);
        }

        // Ÿ���� ����
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(entry));

        // ��� ó��
        if (!string.IsNullOrEmpty(entry.background))
        {
            Sprite bgSprite = GetBackgroundSprite(entry.background);

            if (bgSprite != null)
            {
                backgroundImage.sprite = bgSprite;
            }
        }

        // ���� ó��
        if (!string.IsNullOrEmpty(entry.music))
        {
            AudioClip musicClip = GetMusicClip(entry.music);
            if (musicClip != null && musicSource.clip != musicClip)
            {
                musicSource.clip = musicClip;
                musicSource.Play();
            }
        }

        // SFX ó��
        if (!string.IsNullOrEmpty(entry.sfx))
        {
            AudioClip sfxClip = GetSfxClip(entry.sfx);
            if (sfxClip != null)
            {
                sfxSource.PlayOneShot(sfxClip);
            }
        }

        // ����ϴ� ĳ���� ó��
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

    // ��� Ÿ����
    private IEnumerator TypeText(DialogueEntry entry)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingSound != null)
        {
            typingSound.Play();
        }

        // Ÿ���� ȿ��
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

        // ������ ó��
        if (entry.choices != null || entry.choices.Length > 0)
        {
            ShowChoices(entry.choices);
        }
    }

    // ������
    private void ShowChoices(Choice[] choices)
    {
        DialogueEntry entry = dialogueData.Find(e => e.id == currentID); // ���� ��� (���� �Լ�)

        // ������ UI Ȱ��ȭ
        leftButtonObj.SetActive(true);
        rightButtonObj.SetActive(true);
        leftButtonObjT.SetActive(true);
        rightButtonObjT.SetActive(true);

        // ������ ��ư 1
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

        // ������ ��ư 2
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

    // ���� ��� �Ѿ��
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

            // ������
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

        // ������ ���� �� ���� ���� �̵�
        if (entry.choices == null || entry.choices.Length == 0)
        {
            currentID = entry.nextID;
            ShowEntry(currentID);
        }
    }
}