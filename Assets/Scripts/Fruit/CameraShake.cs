using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 카메라 흔들기 (코루틴)
    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}