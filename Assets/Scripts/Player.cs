using UnityEngine;

// 캐릭터 스프라이트 로드 및 설정
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

            // 스프라이트 크기 기준으로 적당한 스케일 적용
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            // Collider 설정
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = sr.sprite.bounds.size;
            collider.offset = sr.sprite.bounds.center;

            // Sprite 크기에서 pivot 기준으로 위치 조정
            Vector3 spriteOffset = sr.bounds.extents;
        }
        else
        {
            Debug.LogWarning("캐릭터 스프라이트 에러");
        }
    }
}