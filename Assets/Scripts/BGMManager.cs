using UnityEngine;

/// <summary>
/// 씬이 바뀌어도 배경음악(BGM)이 계속 재생되도록 관리하는 BGMManager 클래스.
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