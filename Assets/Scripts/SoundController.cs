using UnityEngine;

/// <summary>
/// ��ư ���� ��Ʈ��
/// </summary>

public class SoundController : MonoBehaviour
{
    public AudioSource button;

    public void buttonSound()
    {
        button.Play();
    }
}
