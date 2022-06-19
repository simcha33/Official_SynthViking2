using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 



public class EnemySpawnManager : MonoBehaviour
{
    [Header ("Lists")]
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<GameObject> allEnemyTypes = new List<GameObject>();
    public List<WaveSpawnData> allWaves = new List<WaveSpawnData>();
    public List<BasicEnemyScript> spawnedAliveEnemies = new List<BasicEnemyScript>();
    public List<BasicEnemyScript> spawnedDeadEnemies = new List<BasicEnemyScript>();

    [Header("Scripts")]
    private EnemyManager enemyBehaviourScript;
    public WaveSpawnData choosenSpawnData;
    public PlayerState playerState;
    public ThirdPerson_PlayerControler playerController;

    [Header("Components")]
    public Transform deadEnemyParent;
    public Transform aliveEnemyParent;

    [Header ("SPAWNING RESTRICTIONS")]
    public int maxEnemiesAllowedInScene;
   // public int enemyCount; 

    //Spawn cooldowns and timers
    public float groupSpawnCooldownDuration;
    public float groupSpawnCooldownTimer;

    [Header("WAVE AREA")]
    public SpawnAreaTrigger waveArea; 

    [Header("ENEMY SPAWNING LOGIC")]
    public int maxGroupSize;
    public int minGroupSize;
    public int totalGroupSize;  

    public int currentEnemyTypeToSpawn;
    private Transform choosenSpawnLocation;
    public bool canSpawnNewEnemies;
    public bool activateSpawner;
    private float maxDeadEnemies = 20f; 
  
    [Header("WAVE LOGIC")]
    public int currentWave;
    public int enemiesLeft; 
    public int maxWaves;
    public int maxEnemiesToSpawnThisWave;
    public int enemiesSpawnedThisWave;
    public float waveCountdownTimer;
    public float waveCountdownDuration;
    public string currentWaveTitle;
    public bool waveHasEnded;
    public bool waveHasStarted;
    public bool canStartNewWave;
    private  float waveTurnOffTimer;
    private float waveTurnOffDuration = 6f;
    private bool waveIsTurnedOff;

    [Header("VISUALS")]
    public GameObject waveUI; 
    public TextMeshPro waveText;
    public TextMeshPro enemyCountText;
   // public TextMeshPro waveInfoText;
    public RawImage skullImage; 

    private enum waveState
    {
        WaveEvent,
        ResetEvent, 
    }

    private int currentWaveState; 


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
        enemyCountText.alpha = 0f;
        skullImage.enabled = false;
        TurnOffWave(); 

    }

    private void Update()
    {
        if (waveArea != null)
        {
            if (waveIsTurnedOff) TurnOnWave(); 
            CheckForWave();
            CheckForCleanUp();
            HandleUI();

            if (waveHasStarted)
            {
                CheckForEnemyCount();
                CheckForEnemySpawn();
            }
        }
        else
        {
            TurnOffWave(); 
        }
    }

    void CheckForEnemyCount()
    {

    }

    void TurnOffWave()
    {
        waveIsTurnedOff = true; 
        waveArea = null;
        waveUI.SetActive(false); 
       // waveText.gameObject.SetActive(false);
      //  enemyCountText.gameObject.SetActive(false);
       // skullImage.gameObject.SetActive(false); 
    }

    void TurnOnWave()
    {
        waveIsTurnedOff = false;
        waveUI.SetActive(true);
        //  waveText.gameObject.SetActive(true);
        //  enemyCountText.gameObject.SetActive(true);
        //  skullImage.gameObject.SetActive(true);

    }

    void CheckForEnemySpawn()
    {

        if (spawnedAliveEnemies.Count < maxEnemiesAllowedInScene && enemiesSpawnedThisWave < maxEnemiesToSpawnThisWave)  //Check if the max cap of enemies has been reached 
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
                    if (enemiesSpawnedThisWave >= maxEnemiesToSpawnThisWave) totalGroupSize = 0;
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
        int choosenSpawner = Random.Range(0, enemySpawners.Count);
        choosenSpawnLocation = enemySpawners[choosenSpawner].spawnPoint; 
    }

    void SpawnEnemy(int enemySpawnType, Transform placeToSpawn)
    {
        GameObject enemyToSpawn = allEnemyTypes[enemySpawnType];
        GameObject enemy = Instantiate(enemyToSpawn, new Vector3(placeToSpawn.position.x, placeToSpawn.position.y, placeToSpawn.position.z), enemyToSpawn.transform.rotation);      
        enemiesSpawnedThisWave++; 
    }

    void CheckForWave()
    {

        if (waveHasStarted) enemyCountText.text = enemiesLeft.ToString(); 

        if (enemiesSpawnedThisWave >=  maxEnemiesToSpawnThisWave|| currentWave == 0) 
        {
            canSpawnNewEnemies = false;
            
            if(enemiesLeft <= 0 || currentWave == 0)
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
        enemiesSpawnedThisWave = 0;
      

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
        playerState.AddHealth(25f);
        enemiesLeft = maxEnemiesToSpawnThisWave; 
    }


    void EndWaves()
    {
        waveTurnOffTimer += Time.deltaTime;
        if (waveTurnOffTimer >= waveTurnOffDuration) TurnOffWave(); 
        canStartNewWave = false;
        waveText.text = "ENEMIES DESTROYED"; 
    }

    void HandleUI()
    {
        if (waveHasStarted && waveText.alpha > 0) waveText.alpha -= Time.deltaTime * .1f;

        if (waveHasStarted && waveText.alpha <= 0 && enemyCountText.alpha < 1)
        {
            enemyCountText.alpha += Time.deltaTime * .35f;
            skullImage.enabled = true; 
        }
        else if (waveHasEnded)
        {
            skullImage.enabled = false;
            enemyCountText.alpha = 0f;
        }
    }

    public void TriggerNewSpawnArea()
    {

        groupSpawnCooldownTimer = groupSpawnCooldownDuration;
        waveCountdownTimer = waveCountdownDuration;
        maxWaves = allWaves.Count;

        currentWave = 0;
        enemiesSpawnedThisWave = 0;

        canStartNewWave = true;
        enemyCountText.alpha = 0f;
        skullImage.enabled = false;
    }

    public void CheckForCleanUp()
    {
        if (spawnedDeadEnemies.Count > maxDeadEnemies)
        {
            spawnedDeadEnemies[0].DestroySelf();
        }
    }

}



