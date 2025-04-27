using UnityEngine;

// Platformer 배경 이동 스크립트
public class ParallaxBackground : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    private Transform currentCam;
    private Vector3 previousCam;
    private float spriteWidth;

    void Start()
    {
        currentCam = Camera.main.transform;
        previousCam = currentCam.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        Parallax();
    }

    // 배경 무한 연결 메서드
    private void Parallax()
    {
        // 매 프레임마다 parallaxSpeed만큼 왼쪽으로 이동
        transform.position += Vector3.left * parallaxSpeed * Time.deltaTime;

        // 배경이 카메라 기준 왼쪽으로 완전히 나가면 오른쪽으로 재배치
        if (transform.position.x < currentCam.position.x - spriteWidth)
        {
            Vector3 newPos = transform.position;
            newPos.x += spriteWidth * 2f;  // 오른쪽 배경 뒤로 이동
            transform.position = newPos;
        }
    }
}
