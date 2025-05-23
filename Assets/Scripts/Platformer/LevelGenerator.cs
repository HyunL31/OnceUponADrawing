using UnityEngine;

/// <summary>
/// �������� ������ ����
/// </summary>

public class LevelGenerator : MonoBehaviour
{
    public StartManager startManager;

    // ������ ������
    public GameObject evenItemPrefab;
    public GameObject oddItemPrefab;

    private float nextSpawnX = 0f;      // ���� �������� ������ X ��ǥ
    private float generatorSpeed = 2f;

    public float spawnDistanceAhead = 15f;      // �ּ� ���� �Ÿ�
    public float minGap = 4f;
    public float maxGap = 6f;

    void Update()
    {
        if (startManager.isStart)
        {
            // ���� ��ü�� ���������� �̵�
            transform.position += Vector3.right * generatorSpeed * Time.deltaTime;

            while (transform.position.x + spawnDistanceAhead > nextSpawnX)
            {
                SpawnItem(nextSpawnX);
            }
        }
    }

    // �������� ������ ����
    void SpawnItem(float xPos)
    {
        float randomY = Random.Range(-1.0f, 1.0f);      // �������� Y ��ġ
        Vector3 itemPos = new Vector3(xPos, randomY, 0);

        GameObject prefabToSpawn;

        // ���� Ȯ���� ¦�� ������ ����
        if (Random.value < 0.7f)
        {
            prefabToSpawn = evenItemPrefab;
        }
        else
        {
            prefabToSpawn = oddItemPrefab;
        }

        Instantiate(prefabToSpawn, itemPos, Quaternion.identity);

        // ���� ������ ���� ��ġ
        float gap = Random.Range(minGap, maxGap);
        nextSpawnX = xPos + gap;
    }
}
