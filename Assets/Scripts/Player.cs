using UnityEngine;

// ĳ���� ��������Ʈ �ε� �� ����
public class Player : MonoBehaviour
{
    void Start()
    {
        Sprite loadSprite = DrawingLoader.LoadPlayerDrawing();

        if (loadSprite != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }

            sr.sprite = loadSprite;

            // ��������Ʈ ũ�� �������� ������ ������ ����
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            // Collider ����
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = sr.sprite.bounds.size;
            collider.offset = sr.sprite.bounds.center;

            // Sprite ũ�⿡�� pivot �������� ��ġ ����
            Vector3 spriteOffset = sr.bounds.extents;
        }
        else
        {
            Debug.LogWarning("ĳ���� ��������Ʈ ����");
        }
    }
}