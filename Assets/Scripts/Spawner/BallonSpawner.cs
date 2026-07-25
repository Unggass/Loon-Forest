using UnityEngine;

public class BallonSpawner : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] GameObject ballonPrefab;

    [Space(15)]

    [Header("Spawn Position Settings")]
    [SerializeField] float minX = 0;
    [SerializeField] float maxX = 0;
    [SerializeField] float minY = 0;
    [SerializeField] float maxY = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BallonSpawn();
    }

    void BallonSpawn()
    {
        float spawnX = Random.Range(minX, maxX);
        float SpawnY = Random.Range(minY, maxY);

        Vector2 spawnPos = new Vector2(spawnX, SpawnY);
        Instantiate(ballonPrefab, spawnPos, Quaternion.identity);
    }
}
