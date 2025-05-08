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
    public Image backgroundImage;
    public List<BackgroundData> backgroundList;

    public Image characterImage;
    public List<CharacterData> characterList;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    // Typing effect
    [SerializeField]
    private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public AudioSource typingSound;

    // Character
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Sprite sprite;
    }

    // Background Image
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

    // Option
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

        // for Minigame
        if (PlayerPrefs.HasKey("ResumeID"))
        {
            currentID = PlayerPrefs.GetInt("ResumeID");
            PlayerPrefs.DeleteKey("ResumeID");
        }

        ShowEntry(currentID);

        // Adding player character
        Sprite playerSprite = DrawingLoader.LoadPlayerDrawing();

        if (playerSprite != null)
        {
            characterList.Add(new CharacterData { name = "ª°∞£ ∏¡≈‰", sprite = playerSprite });
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextButton();
        }
    }

    // Get JSON File
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

        // Minigame
        if (!string.IsNullOrEmpty(entry.command))
        {
            // Save restart entry
            if (entry.nextID >= 0)
            {
                PlayerPrefs.SetInt("ResumeID", entry.nextID);
                Debug.Log($"[DialogueManager] Saved return ID {entry.nextID} before loading minigame");
            }

            // Move to Minigame
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

            return;
        }

        // Dialogue
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(entry));

        // Background
        if (!string.IsNullOrEmpty(entry.background))
        {
            Sprite bgSprite = GetBackgroundSprite(entry.background);

            if (bgSprite != null)
            {
                backgroundImage.sprite = bgSprite;
            }
        }

        // Character
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

    // Typing Dialogue
    private IEnumerator TypeText(DialogueEntry entry)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingSound != null)
        {
            typingSound.Play();
        }

        // Typing effect
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

        // it next entry has option route
        if (entry.choices != null || entry.choices.Length > 0)
        {
            ShowChoices(entry.choices);
        }
    }

    // Option
    private void ShowChoices(Choice[] choices)
    {
        DialogueEntry entry = dialogueData.Find(e => e.id == currentID); // current dialogue

        // Active Option UI
        leftButtonObj.SetActive(true);
        rightButtonObj.SetActive(true);
        leftButtonObjT.SetActive(true);
        rightButtonObjT.SetActive(true);

        // Option Button 1
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

        // Option button 2
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

    // Show next dialogue
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

            // Option
            if (entry.choices != null && entry.choices.Length > 0)
            {
                ShowChoices(entry.choices);
            }

            return;
        }

        // If no option, move to next entry
        if (entry.choices == null || entry.choices.Length == 0)
        {
            currentID = entry.nextID;
            ShowEntry(currentID);
        }
    }
}