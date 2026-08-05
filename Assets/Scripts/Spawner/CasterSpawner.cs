using UnityEngine;

public class CasterSpawner : MonoBehaviour
{
    [SerializeField] GameObject casterPrefab;

    [SerializeField] Vector2 casterLeftSpawn;
    [SerializeField] Vector2 casterRightSpawn;

    [SerializeField] float minSpawnInterval = 3f;
    [SerializeField] float maxSpawnInterval = 6f;

    [HideInInspector] public bool isSpawning = false;

    float timer;

    // Update is called once per frame
    void Update()
    {
        if (isSpawning) { return; }

        timer += Time.deltaTime;

        float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

        // Spawn Caster
        if (timer > spawnInterval * 2)
        {
            bool spawnCaster = Random.value < 0.2 ? true : false;

            if (spawnCaster)
            {
                timer = 0f;
                isSpawning = true;
                CasterSpawn();
            }
        }
    }

    void CasterSpawn()
    {
        float sign = Random.value < 0.5 ? 1 : -1;

        switch (sign)
        {
            case 1:
                Instantiate(casterPrefab, casterRightSpawn, Quaternion.identity);
                break;
            case -1:
                Instantiate(casterPrefab, casterLeftSpawn, Quaternion.identity);
                break;
        }
    }
}
