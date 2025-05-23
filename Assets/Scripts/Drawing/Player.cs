using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ������ �÷��̾� ĳ���� sprite �ε�
/// </summary>

public class Player : MonoBehaviour
{
    void Start()
    {
        Sprite loadSprite = DrawingLoader.LoadPlayerDrawing();

        if (loadSprite != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            sr.sprite = loadSprite;

            // �̷ο��� ��� ��, ĳ���� ũ�� ����
            if (SceneManager.GetActiveScene().name == "Maze")
            {
                transform.localScale = new Vector3(0.12f, 0.12f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            }

            // ĳ������ ������ �������� �ݶ��̴� ����
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();

            collider.size = sr.sprite.bounds.size;
            collider.offset = sr.sprite.bounds.center;

            Vector3 spriteOffset = sr.bounds.extents;
        }
    }
}