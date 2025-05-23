using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 내에서 플레이어 캐릭터 sprite 로드
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

            // 미로에서 사용 시, 캐릭터 크기 설정
            if (SceneManager.GetActiveScene().name == "Maze")
            {
                transform.localScale = new Vector3(0.12f, 0.12f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            }

            // 캐릭터의 범위를 기준으로 콜라이더 설정
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();

            collider.size = sr.sprite.bounds.size;
            collider.offset = sr.sprite.bounds.center;

            Vector3 spriteOffset = sr.bounds.extents;
        }
    }
}