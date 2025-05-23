using UnityEngine;

/// <summary>
/// ���� �ٲ� �������(BGM)�� ��� ����ǵ��� �����ϴ� BGMManager Ŭ����.
/// </summary>

public class BGMManager : MonoBehaviour
{
    public AudioSource bgm;

    void Awake()
    {
        bgm = GetComponent<AudioSource>();

        if (bgm != null)
        {
            DontDestroyOnLoad(bgm);
        }
    }
}