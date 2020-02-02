using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public List<GameObject> spawnList;
    public float spawnChance = 0.8f;
    
    void Start()
    {
        if(Random.Range(0, 1) <= spawnChance)
        {
            Instantiate(spawnList[Mathf.RoundToInt(Random.Range(1, 1000000)) % spawnList.Count], transform);
        }
    }

   
    void Update()
    {
        
    }
}
