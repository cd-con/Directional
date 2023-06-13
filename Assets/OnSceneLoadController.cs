using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoadController : MonoBehaviour
{
    public Vector3 startScale;
    public EnemySpawner[] spawners;
    void Start()
    {
        startScale = transform.localScale;
        transform.localScale = Vector3.one * 2;
        transform.position = PlayerController.Instance.playerTransform.position;
    }
    bool spawnCalled = false;
    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime * 2.5f);
        Debug.Log((Vector3.one * 2 - transform.localScale).magnitude);
        if ((Vector3.one * 2 - transform.localScale).magnitude > 1.5 && !spawnCalled)
        {
            spawnCalled = true;
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.Spawn();
            }
        }
    }
}
