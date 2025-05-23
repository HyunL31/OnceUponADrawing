using UnityEngine;

/// <summary>
/// 랜덤으로 아이템 생성
/// </summary>

public class LevelGenerator : MonoBehaviour
{
    public StartManager startManager;

    // 아이템 프리팹
    public GameObject evenItemPrefab;
    public GameObject oddItemPrefab;

    private float nextSpawnX = 0f;      // 다음 아이템을 생성할 X 좌표
    private float generatorSpeed = 2f;

    public float spawnDistanceAhead = 15f;      // 최소 생성 거리
    public float minGap = 4f;
    public float maxGap = 6f;

    void Update()
    {
        if (startManager.isStart)
        {
            // 레벨 자체가 오른쪽으로 이동
            transform.position += Vector3.right * generatorSpeed * Time.deltaTime;

            while (transform.position.x + spawnDistanceAhead > nextSpawnX)
            {
                SpawnItem(nextSpawnX);
            }
        }
    }

    // 랜덤으로 아이템 스폰
    void SpawnItem(float xPos)
    {
        float randomY = Random.Range(-1.0f, 1.0f);      // 아이템의 Y 위치
        Vector3 itemPos = new Vector3(xPos, randomY, 0);

        GameObject prefabToSpawn;

        // 일정 확률로 짝수 아이템 생성
        if (Random.value < 0.7f)
        {
            prefabToSpawn = evenItemPrefab;
        }
        else
        {
            prefabToSpawn = oddItemPrefab;
        }

        Instantiate(prefabToSpawn, itemPos, Quaternion.identity);

        // 다음 아이템 생성 위치
        float gap = Random.Range(minGap, maxGap);
        nextSpawnX = xPos + gap;
    }
}
