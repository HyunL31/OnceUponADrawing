using UnityEngine;

/// <summary>
/// 버튼 사운드 컨트롤
/// </summary>

public class SoundController : MonoBehaviour
{
    public AudioSource button;

    public void buttonSound()
    {
        button.Play();
    }
}
