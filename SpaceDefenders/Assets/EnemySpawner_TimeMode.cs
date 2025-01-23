using UnityEngine;
using System.Collections.Generic;
using SpaceDefenders.GameManagement;

public class EnemySpawner_TimeMode : MonoBehaviour
{
    [SerializeField] private TimeModeGameManager manager;

    private List<GameObject> spawnedEnemys = new List<GameObject>();

    [SerializeField] private GameObject enemy;

    [SerializeField] private int spawnWhenEnemysAlive = 1;
    [SerializeField] private int enemysPerWave = 2;

    int enemysAtPreviousFrame = 0;

    void Update()
    {
        //remove every destroyed enemy from the list
        foreach (var enemy in spawnedEnemys)
        {
            if (enemy == null)
            {
                spawnedEnemys.Remove(enemy);
                break;
            }
        }

        if(spawnedEnemys.Count < enemysAtPreviousFrame)
        {
            manager.enemysKilled += enemysAtPreviousFrame - spawnedEnemys.Count;
        }

        if(spawnedEnemys.Count <= spawnWhenEnemysAlive)
        {
            SpawnEnemys();
        }


        enemysAtPreviousFrame = spawnedEnemys.Count;
    }

    private void SpawnEnemys()
    {
        for (int i = 0; i < enemysPerWave; i++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * 25;
            spawnedEnemys.Add(Instantiate(enemy, spawnPos, Quaternion.identity));
        }
    }
}