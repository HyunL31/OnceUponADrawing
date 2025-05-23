using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� (��ȭ �� ���� Īȣ �ý���)
/// </summary>

public class MyPageManager : MonoBehaviour
{
    public FameSystem fameSystem;

    public Transform itemListParent;
    public GameObject infoItemPrefab;

    public GameObject titlePanel;

    void Start()
    {
        AddStoryItems();
    }

    // �÷��̾ ������ ��ȭ���� Info Item UI ����
    void AddStoryItems()
    {
        List<string> storyCodes = StoryUnlockManager.Instance.GetUnlockedStoryCodes();

        foreach (string code in storyCodes)
        {
            var storyData = fameSystem.GetStoryData(code);

            if (storyData == null)
            {
                continue;
            }

            // ��ȭ�� ���� ��������
            string personality = PlayerPrefs.GetString($"StoryPersonality{code}", "Kind");      // �⺻�� Kind

            // ���� ��� Īȣ
            var titleData = fameSystem.GetTitleData($"{personality}");

            // Īȣ �̸�
            string description = personality switch
            {
                "Kind" => "������ ������ ��ȣ��",
                "Brave" => "�η��� ���� ���� ���",
                "Curious" => "������ ȣ����� Ž�谡",
                _ => "�� �� ���� ����"
            };

            // ���� ���� �ؽ�Ʈ
            string subText = personality switch
            {
                "Kind" => "�̾߱� �� ����� �����ϰ� ��� ���� �ι��̾����.",
                "Brave" => "���� �տ����� ��� ���� ����� �λ� �����ϴ�.",
                "Curious" => "�����̵� �ñ����ϸ� ������ ������ ����� ����!",
                _ => "�� �� ���� ����"
            };

            // ��ȭ ����� & ���� ������
            Sprite storyTitle = Resources.Load<Sprite>($"Icons/{code}");
            Sprite icon = Resources.Load<Sprite>($"Icons/{personality}");

            AddInfoItem(storyTitle, storyData.displayName, icon, description, subText);
        }
    }

    void AddInfoItem(Sprite storyImage, string storyTitle, Sprite personalIcon, string personalTitle, string subText)
    {
        GameObject obj = Instantiate(infoItemPrefab, itemListParent);
        InfoItem item = obj.GetComponent<InfoItem>();

        if (item != null)
        {
            item.Init(
                icon: personalIcon,
                title: storyTitle,
                image: storyImage,
                desc: subText,
                personality: personalTitle
            );
        }
    }

    // ��ư ��Ʈ��
    public void Back()
    {
        titlePanel.SetActive(false);
    }

    public void Title()
    {
        titlePanel.SetActive(true);
    }
}
