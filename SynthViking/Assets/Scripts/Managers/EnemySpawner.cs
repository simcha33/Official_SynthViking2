using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    EnemyManager enemyBehaviourManagerScript;
    EnemySpawnManager enemySpawnManagerScript;
    public Transform spawnPoint;
    public EnemySpawner thisScript; 
   // Vector3 spawnDirection; 

    void Awake()
    {
        enemySpawnManagerScript = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();
        enemyBehaviourManagerScript = GameObject.Find("EnemyBehaviourManager").GetComponent<EnemyManager>(); 
    }

    private void Start()
    {
        thisScript = GetComponent<EnemySpawner>(); 
        enemySpawnManagerScript.enemySpawners.Add(thisScript); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
