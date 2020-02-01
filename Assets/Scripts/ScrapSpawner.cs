using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapSpawner : MonoBehaviour
{
    public float spawnRate = 1.0f;
    public List<GameObject> prefabs;
    private int max_spawn_range = 5;


    private float _lastSpawnTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (/* tuka proverka za game over && */ Time.time >= _lastSpawnTime + spawnRate)
        {
            SpawnScrap();
            _lastSpawnTime = Time.time;
        }
    }

    private void SpawnScrap()
    {
        
        GameObject prefab_to_spawn = GetRandomScrapPrefab();
        Vector3 spawnPosition = GetNewSpawnPosition();
        Renderer rend = prefab_to_spawn.GetComponent<Renderer>();
        Vector3 size = rend.bounds.size;
        Instantiate(prefab_to_spawn, spawnPosition, Quaternion.identity);
    }

    private GameObject GetRandomScrapPrefab()
    {
        int randomNum = Random.Range(0, 10000) % prefabs.Count;
        return prefabs[randomNum];
    }

    private GameObject GetRandomPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int randomNum = Random.Range(0, 10000) % players.Length;
        return players[randomNum];
    }

    private Vector3 GetNewSpawnPosition()
    {
        GameObject player = GetRandomPlayer();
        if (player == null)
        {
            Debug.Log("NO PLAYERS");
            return new Vector3();
        }
        Vector3 targetDirection = player.transform.position;
        Vector3 result = transform.position = RandomPointOnCircleEdge(max_spawn_range) + targetDirection;
        return result;
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
