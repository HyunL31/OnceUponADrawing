using UnityEngine;

// ���� �ӵ��� �������� �����̱�
public class MoveLeft : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving = true;

    void Update()
    {
        if (!isMoving) return;

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    public void Stop()
    {
        isMoving = false;
    }

}