using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 



public class EnemySpawnManager : MonoBehaviour
{
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<GameObject> allEnemyTypes = new List<GameObject>();
    public List<WaveSpawnData> allWaves = new List<WaveSpawnData>(); 
    private EnemyManager enemyBehaviourScript;
    public WaveSpawnData choosenSpawnData; 

    [Header ("SPAWNING RESTRICTIONS")]
    public int maxEnemiesAllowedInScene;
    public int currentEnemiesInScene;

    //Spawn cooldowns and timers
    public float groupSpawnCooldownDuration;
    public float groupSpawnCooldownTimer;

    [Header("ENEMY SPAWNING LOGIC")]
    public int maxGroupSize;
    public int minGroupSize;
    public int totalGroupSize;  

    public int currentEnemyTypeToSpawn;
    private Transform choosenSpawnLocation;
    public bool canSpawnNewEnemies;
    public bool activateSpawner;
  

    [Header("WAVE LOGIC")]
    public int currentWave;
    public int maxWaves;
    public int maxEnemiesToSpawnThisWave;
    public int currentEnemiesSpawnedThisWave;
    public float waveCountdownTimer;
    public float waveCountdownDuration;
    public string currentWaveTitle;
    public bool waveHasEnded;
    public bool waveHasStarted;
    public bool canStartNewWave;

    [Header("VISUALS")]
    public TextMeshPro waveText;
    public TextMeshPro enemyCountText; 


    private void Awake()
    {
        enemyBehaviourScript = GameObject.Find("EnemyBehaviourManager").GetComponent<EnemyManager>();
    }

    private void Start()
    {
        groupSpawnCooldownTimer = groupSpawnCooldownDuration;
        waveCountdownTimer = waveCountdownDuration; 
        maxWaves = allWaves.Count;
        currentWave = 0;
        canStartNewWave = true; 
    }

    private void Update()
    {
        CheckForWave();
        HandleUI(); 

        if (waveHasStarted)
        {
            CheckForEnemyCount();
            CheckForEnemySpawn();
        }
    }


    void CheckForEnemyCount()
    {
        currentEnemiesInScene = enemyBehaviourScript.currentenemiesInScene.Count; 
    }

    void CheckForEnemySpawn()
    {

        if (currentEnemiesInScene < maxEnemiesAllowedInScene && currentEnemiesSpawnedThisWave < maxEnemiesToSpawnThisWave)  //Check if the max cap of enemies has been reached 
        {
            canSpawnNewEnemies = true;
        }
        else
        {
            canSpawnNewEnemies = false;
        }
      
       
        if (canSpawnNewEnemies)  //If we can spawn enemies start the wave spawn cooldown timer 
        {
            groupSpawnCooldownTimer -= Time.deltaTime;

            if(groupSpawnCooldownTimer <= 0) //A new group is allowed to spawn 
            {
                totalGroupSize = Random.Range(minGroupSize, maxGroupSize);

                for (int i = 0; i < totalGroupSize; i++)
                {
                    SetEnemyToSpawnType();
                    SetSpawnLocation();
                    SpawnEnemy(currentEnemyTypeToSpawn, choosenSpawnLocation);
                }

                groupSpawnCooldownTimer = groupSpawnCooldownDuration;
            }
        }
    }

  

    void SetEnemyToSpawnType()
    {
        currentEnemyTypeToSpawn = (int)EnemyTypes.Grunt;       
    }

    void SetSpawnLocation()
    {
        int choosenSpawner = Random.Range(0, enemySpawners.Count + 1);
        choosenSpawnLocation = enemySpawners[choosenSpawner].spawnPoint; 
    }

    void SpawnEnemy(int enemySpawnType, Transform placeToSpawn)
    {
        GameObject enemyToSpawn = allEnemyTypes[enemySpawnType];
        Instantiate(enemyToSpawn, new Vector3(placeToSpawn.position.x, placeToSpawn.position.y, placeToSpawn.position.z), enemyToSpawn.transform.rotation); 
        currentEnemiesSpawnedThisWave++; 
    }

    void CheckForWave()
    {
        if(currentEnemiesSpawnedThisWave >=  maxEnemiesToSpawnThisWave|| currentWave == 0) 
        {
            canSpawnNewEnemies = false;
            
            if(currentEnemiesInScene <= 0 || currentWave == 0)
            {
                if (currentWave > 0) waveText.text = "WAVE CLEARED";

                waveText.alpha = 1f; 
                waveHasEnded = true;
                if (currentWave < maxWaves) DoNextWaveTranstition();              
                else EndWaves(); 
            }
        }
    }

    void SetNextWave()
    {
        currentWave++;
        choosenSpawnData = allWaves[currentWave - 1]; 
        maxEnemiesAllowedInScene = choosenSpawnData.maxEnemiesInScene; //Max enemies allowed in the scene at the same time 
        maxEnemiesToSpawnThisWave = choosenSpawnData.maxEnemiesInWave; //Max enemies to spawn in a wave 
        minGroupSize = choosenSpawnData.minGroupSpawnSize;  //max enimes to spawn in a group 
        maxGroupSize  = choosenSpawnData.maxGroupSpawnSize; //Min enemies to spawn in a group
        currentWaveTitle = choosenSpawnData.WaveTitle;
        currentEnemiesSpawnedThisWave = 0;
    }

    void DoNextWaveTranstition()
    {
        waveCountdownTimer -= Time.deltaTime;
        waveText.text = waveCountdownTimer.ToString("F0");

        if (waveCountdownTimer <= 0 && canStartNewWave)
        {   
            SetNextWave();
            StartNewWave();
            waveCountdownTimer = waveCountdownDuration;
        }
    }

    void StartNewWave()
    {
        canSpawnNewEnemies = true;
        waveHasEnded = false;
        waveHasStarted = true;
        groupSpawnCooldownTimer = groupSpawnCooldownDuration;
        waveText.text = "ACT " + currentWave + " " + currentWaveTitle; 
    }

    void EndWaves()
    {
        canStartNewWave = false;
        waveText.text = "ENEMIES DESTROYED"; 
    }

    void HandleUI()
    {
        if (waveHasStarted) waveText.alpha -= Time.deltaTime * .1f; 
    }
}

 
