using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab;
    public int enemyHeapCount;

    public void Spawn()
    {
        for (int i = 0; i < enemyHeapCount; i++)
        {
            Instantiate(prefab, new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) + (Vector2)transform.position, Quaternion.identity);
        }
    }
}
