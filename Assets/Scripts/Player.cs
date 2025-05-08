using UnityEngine;

/// <summary>
/// Player Character Sprite Load
/// </summary>

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

            // Setting character spirte scale
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            // Setting collider
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = sr.sprite.bounds.size;
            collider.offset = sr.sprite.bounds.center;


            Vector3 spriteOffset = sr.bounds.extents;
        }
        else
        {
            Debug.LogWarning("Character Sprite Error");
        }
    }
}