using UnityEngine;

// Platformer ��� �̵� ��ũ��Ʈ
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

    // ��� ���� ���� �޼���
    private void Parallax()
    {
        // �� �����Ӹ��� parallaxSpeed��ŭ �������� �̵�
        transform.position += Vector3.left * parallaxSpeed * Time.deltaTime;

        // ����� ī�޶� ���� �������� ������ ������ ���������� ���ġ
        if (transform.position.x < currentCam.position.x - spriteWidth)
        {
            Vector3 newPos = transform.position;
            newPos.x += spriteWidth * 2f;  // ������ ��� �ڷ� �̵�
            transform.position = newPos;
        }
    }
}
