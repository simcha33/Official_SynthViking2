using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaTrigger : MonoBehaviour
{
    public List<WaveSpawnData> allWaves = new List<WaveSpawnData>();
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<GameObject> allEnemyTypes = new List<GameObject>();
    public EnemySpawnManager spawnManager;
    public EnemyManager enemyManager;
    public float waveCountdownDuration = 10f; 
    public bool spawnAreaIsActive;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !spawnAreaIsActive)
        {
            spawnAreaIsActive = true;
            SetNewSpawnArea(); 
        }
    }

    void SetNewSpawnArea()
    {

        if (spawnManager.spawnedDeadEnemies.Count > 0)
        {
            foreach (BasicEnemyScript enemy in spawnManager.spawnedDeadEnemies)
            {
                enemy.DestroySelf();
            }
        }

        if (spawnManager.spawnedAliveEnemies.Count > 0)
        {
            foreach (BasicEnemyScript enemy in spawnManager.spawnedAliveEnemies)
            {           
                enemy.DestroySelf();
            }
        }

      
        spawnManager.spawnedDeadEnemies.Clear();
        spawnManager.spawnedAliveEnemies.Clear();
        spawnManager.waveArea = GetComponent<SpawnAreaTrigger>();
        spawnManager.allWaves = allWaves;
        spawnManager.waveCountdownDuration = waveCountdownDuration; 
        spawnManager.enemySpawners = enemySpawners;
        spawnManager.allEnemyTypes = allEnemyTypes; 

        spawnManager.TriggerNewSpawnArea(); 
    }
}
