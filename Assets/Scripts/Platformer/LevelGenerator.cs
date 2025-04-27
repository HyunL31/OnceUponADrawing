using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject itemPrefab;

    private float nextSpawnX = 0f;
    private float generatorSpeed = 2f;

    public float spawnDistanceAhead = 15f;
    public float minGap = 5f;
    public float maxGap = 8f;

    void Update()
    {
        // 레벨 자체가 오른쪽으로 살짝 이동
        transform.position += Vector3.right * generatorSpeed * Time.deltaTime;

        while (transform.position.x + spawnDistanceAhead > nextSpawnX)
        {
            SpawnItem(nextSpawnX);
        }
    }

    // 랜덤으로 아이템 스폰
    void SpawnItem(float xPos)
    {
        float randomY = Random.Range(-1.5f, 1.0f);

        Vector3 itemPos = new Vector3(xPos, randomY, 0);
        Instantiate(itemPrefab, itemPos, Quaternion.identity);

        float gap = Random.Range(minGap, maxGap);
        nextSpawnX = xPos + gap;
    }
}
