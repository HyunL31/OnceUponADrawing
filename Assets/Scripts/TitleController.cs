using Unity.Hierarchy;
using UnityEngine;

// 각 동화의 Title 제어 스크립트
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
// 수정: 서서히 나타나기, 사라지기
