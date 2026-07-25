using UnityEngine;
using System.Collections.Generic;

public class DisturberSpawner : MonoBehaviour
{
    #region Spawner Seetings
    public enum DisturberType { Ground, Air}

    [System.Serializable]
    public struct DisturberEntry
    {
        public GameObject prefab;
        public DisturberType type;
    }

    [Header("Object References"), Space(25)]
    [SerializeField] List<DisturberEntry> disturberPrefabs;
    [SerializeField] Transform player;

    [Space (25)]

    [Header ("Ground Spawn Settings")]
    [SerializeField] float groundMinDistance = 15f;
    [SerializeField] float groundMaxDistance = 1f;
    [SerializeField] float groundSpawnY = 0f;

    [Space(10)]

    [Header("Air Spawn Settings")]
    [SerializeField] float airMinX = 0f;
    [SerializeField] float airMaxX = 0f;
    [SerializeField] float airMinY = 0f;
    [SerializeField] float airMaxY = 0f;

    [Space(10)]

    [Header ("Arena Boundary")]
    [SerializeField] float arenaMinX = 1f;
    [SerializeField] float arenaMaxX = 1f;
    [SerializeField] float arenaMinY = 1f;
    [SerializeField] float arenaMaxY = 1f;

    [Space(10)]
    
    [Header ("Spawn Timer")]
    [SerializeField] float spawnInterval = 3f;

    [Space(10)]

    [Header("Difficulty Settings")]
    [SerializeField] float minSpawnInterval = 0.5f;
    [SerializeField] float intervalReduction = 0.5f;
    #endregion

    float timer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            timer = 0f;
            SpawnRandom();
        }
    }

    #region Spawner
    void SpawnRandom()
    {
        // Buat Nilai random terhadap jumlah Prefab yang ada
        int index = Random.Range(0, disturberPrefabs.Count);
        // ambil index prefab
        DisturberEntry chosen = disturberPrefabs[index];

        // paggil spawner berdasarkan tipe prefab
        switch (chosen.type)
        {
            case DisturberType.Ground:
                GroundSpawn(chosen.prefab);
                break;

            case DisturberType.Air:
                AirSpawn(chosen.prefab);
                break;
        }
    }

    void GroundSpawn(GameObject prefab)
    {
        // Buat Nilai random untk jarak yang akan digunakan nanti
        float direction = Random.Range(groundMinDistance, groundMaxDistance);
        float sign = Random.value < 0.5 ? 1 : -1;

        // Posisi player ditambahkan dengan jarak yang telah dihitung Sebelumnya
        float spawnX = player.position.x + (direction * sign);
        // Lalu Clamp posisinya dengan Batasan Arena Arena
        spawnX = Mathf.Clamp(spawnX, arenaMinX, arenaMaxX);

        // Instantiate Prefab
        Vector2 spawnPos = new Vector2(spawnX, groundSpawnY);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    } 

    void AirSpawn(GameObject prefab)
    {
        // buat nilai random dari Posisi SpawnX dan SpawnY
        float spawnX = Random.Range(airMinX, airMaxX);
        float spawnY = Random.Range(airMinY, airMaxY);

        // Clamp jarak SpawnX dan SpawnX terhadap Batasan Arena
        spawnX = Mathf.Clamp(spawnX, arenaMinX, arenaMaxX);
        spawnY = Mathf.Clamp(spawnY, arenaMinY, arenaMaxY);

        // Instantiate Prefab
        Vector2 spawnPos = new Vector2(spawnX, spawnY);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
    #endregion

    #region Difficulty Settings
    private void OnEnable()
    {
        GameManager.DifficultyIncrease += OnDifficultyIncrease;
    }

    private void OnDisable()
    {
        GameManager.DifficultyIncrease -= OnDifficultyIncrease;
    }

    private void OnDifficultyIncrease(int stepsPassed)
    {
        spawnInterval = Mathf.Max(
            minSpawnInterval,
            spawnInterval - (intervalReduction * stepsPassed)
        );

        Debug.Log($"Spawn interval sekarang: {spawnInterval}s");
    }
    #endregion
}
