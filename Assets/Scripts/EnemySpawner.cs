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

    class SpawnPointHelper
    {
        public Transform point;
        public float maxCooldown;

        public SpawnPointHelper(Transform p, float cooldown)
        {
            point = p;
            _cooldown = maxCooldown = cooldown;
        }

        public void SubtractFromCooldown(float time)
        {
            _cooldown -= time;
        }
        public void ResetCooldown()
        {
            _cooldown = maxCooldown;
        }
        public bool IsReady()
        {
            return _cooldown <= 0.0f;
        }

        float _cooldown = 0.0f;

    }
    public List<Spawnable> spawnables = new List<Spawnable>();
    public bool canSpawn = true;
    public float spawnPointCooldown = 0.5f;

    List<SpawnPointHelper> spawnPoints = new List<SpawnPointHelper>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var sp in spawnables)
            sp.SetOriginalSpawnTime();

        var spawns = GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in spawns)
            spawnPoints.Add(new SpawnPointHelper(s.transform, spawnPointCooldown));
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
        foreach (var s in spawnPoints)
            s.SubtractFromCooldown(Time.deltaTime);
    }

    void SpawnEnemy(Spawnable spawnable)
    {
        for (int i = 0; i < spawnable.spawnAmount; i++)
        {
            SpawnPointHelper selectedSpawnPoint = null;
            //change to coroutine?
            while (true)
            {
                selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                if (selectedSpawnPoint.IsReady())
                {
                    selectedSpawnPoint.ResetCooldown();
                    break;
                }
                else
                    Debug.Log("Cooldown");
            }
            var pos = selectedSpawnPoint.point.position;
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
