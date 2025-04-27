using Unity.Hierarchy;
using UnityEngine;

// �� ��ȭ�� Title ���� ��ũ��Ʈ
public class TitleController : MonoBehaviour
{
    public float destroyDelay = 3.0f;

    void Start()
    {
        gameObject.SetActive(false);

        TitleShow();

        Invoke("TitleDestroy", destroyDelay);
    }

    private void TitleShow()
    {
        gameObject.SetActive(true);
    }

    private void TitleDestroy()
    {
        gameObject.SetActive(false);
    }
}
// ����: ������ ��Ÿ����, �������
