using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerSurvival : MonoBehaviour
{
    private readonly List<GameObject> spawnedEnemies = new();

    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform player;

    [SerializeField] private int spawnWhenEnemiesAlive = 3;
    [SerializeField] private int enemiesPerWave = 5;
    
    void Update()
    {
        //remove every destroyed enemy from the list
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy == null)
            {
                spawnedEnemies.Remove(enemy);
                break;
            }
        }

        if(spawnedEnemies.Count <= spawnWhenEnemiesAlive)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 spawnPos = 20 * Random.insideUnitCircle + (Vector2)player.position;
            spawnedEnemies.Add(Instantiate(enemy, spawnPos, Quaternion.identity));
        }
    }
}
