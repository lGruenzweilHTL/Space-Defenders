using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public WaveData[] waves;
    
    int wave = 0;

    List<GameObject> enemysInWave = new();

    void Update()
    {
        if (!waves[wave].hasBeenTriggered)
        {
            foreach (GameObject enemy in waves[wave].enemys)
            {
                SpawnSingleEnemy(enemy);
            }
            waves[wave].hasBeenTriggered = true;
        }

        //Clear all killed enemys from the list
        foreach (var enemy in enemysInWave)
        {
            if (enemy == null) enemysInWave.Remove(enemy);
        }

        //if wave is cleared, spawn new wave
        if (enemysInWave.Count == 0) wave++;
    }
    void SpawnSingleEnemy(GameObject enemy)
    {
        int spawnQuadrant = Random.Range(0, 4); 

        Vector3 spawnPos = Vector3.zero;
        spawnPos.z = 0f;

        #region set the spawnPos based on the quadrant
        if (spawnQuadrant == 0 || spawnQuadrant == 2)
        {
            spawnPos.x = Random.Range(-25, 25);
            if(spawnQuadrant == 0)
            {
                spawnPos.y = 27;
            }
            else
            {
                spawnPos.y = -27;
            }
        }
        if(spawnQuadrant == 1 || spawnQuadrant == 3)
        {
            spawnPos.y = Random.Range(-25, 25);
            if(spawnQuadrant == 1)
            {
                spawnPos.x = 27;
            }
            else
            {
                spawnPos.x = -27;
            }
        }
        #endregion

        enemysInWave.Add(Instantiate(enemy, spawnPos, Quaternion.identity));
    }
}
[System.Serializable]
public struct WaveData
{
    public GameObject[] enemys;
    [HideInInspector] public bool hasBeenTriggered;
}