using UnityEngine;

/// <summary>
/// ÇÃ·§Æû ¿ÞÂÊÀ¸·Î ¿òÁ÷ÀÌ±â
/// </summary>

public class MoveLeft : MonoBehaviour
{
    public StartManager startManager;

    public float moveSpeed = 5f;
    private bool isMoving = true;

    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        if (startManager.isStart)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    public void Stop()
    {
        isMoving = false;
    }
}