using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Spawnable
    {
        public GameObject prefab;
        [Tooltip("A base offset for the x axis when spawning")]
        public float xAxisBias = 0.0f;
        [Tooltip("A base offset for the y axis when spawning")]
        public float yAxisBias = 0.0f;

        public float spawnInterval;
        float originalSpawnTime;
        public int spawnAmount = 1;

        public void SetOriginalSpawnTime()
        {
            originalSpawnTime = spawnInterval;
        }
        public void ResetSpawnTime()
        {
            spawnInterval = originalSpawnTime;
        }
    }
    public List<Spawnable> spawnables = new List<Spawnable>();
    public bool canSpawn = true;

    List<Transform> spawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var sp in spawnables)
            sp.SetOriginalSpawnTime();

        var spawns = GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in spawns)
            spawnPoints.Add(s.transform);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawnables.Count; i++)
        {
            spawnables[i].spawnInterval -= Time.deltaTime;
            if (spawnables[i].spawnInterval <= 0.0f)
            {
                spawnables[i].ResetSpawnTime();
                SpawnEnemy(spawnables[i]);
            }
        }
    }

    void SpawnEnemy(Spawnable spawnable)
    {
        for (int i = 0; i < spawnable.spawnAmount; i++)
        {
            var pos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            pos.z = 0.0f;
            GameObject.Instantiate(spawnable.prefab, pos, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var gname = other.gameObject.name;

        if (gname.Contains("Bullet"))
            return;
        if (!other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<EntityHealth>()?.TakeDamage(999);
            return;
        }
        else if (gname.Contains("Player"))
            other.GetComponent<PlayerController>().TakeDamage(999);
    }
}
