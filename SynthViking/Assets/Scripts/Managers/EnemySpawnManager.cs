using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 



public class EnemySpawnManager : MonoBehaviour
{
    [Header ("Lists")]
    #region 
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<GameObject> allEnemyTypes = new List<GameObject>();
    public List<GameObject> enemySpawnList = new List<GameObject>(); 
    public List<WaveSpawnData> allWaves = new List<WaveSpawnData>();
    public List<GameObject> spawnedAliveEnemies = new List<GameObject>();
    public List<BasicEnemyScript> spawnedDeadEnemies = new List<BasicEnemyScript>();
    public List<GameObject> objectSpawnList = new List<GameObject>(); 
      public List<GameObject> objectsInScene = new List<GameObject>(); 
    #endregion 

    [Header("Scripts")]
    #region 
    private EnemyManager enemyBehaviourScript;
    public WaveSpawnData choosenSpawnData;
    public eventManagerScript eventScript; 
    public PlayerState playerState;
    public ThirdPerson_PlayerControler playerController;
        #endregion 


    [Header("Components")]
    #region 
    public Transform deadEnemyParent;
    public Transform aliveEnemyParent;
        #endregion 


    [Header ("SPAWNING RESTRICTIONS")]
    #region 
    public int maxEnemiesAllowedInScene;
   // public int enemyCount; 

    //Spawn cooldowns and timers
    public float groupSpawnCooldownDuration;
    public float groupSpawnCooldownTimer;
    public bool canSpawnEyeballs;
    public bool canSpawnLasers;
    public bool canSpawnPilars;
    #endregion


    [Header("WAVE AREA")]
    #region 
    public SpawnAreaTrigger waveArea; 
        #endregion 


    [Header("ENEMY SPAWNING LOGIC")]
        #region 
    public int maxGroupSize;
    public int minGroupSize;
    public int totalGroupSize;  

    public int currentEnemyToSpawn;
    private Transform choosenSpawnLocation;
    public bool canSpawnNewEnemies;
    public bool activateSpawner;
    private float maxDeadEnemies = 20f; 
    public int maxGruntsToSpawn; 
    public int maxBigGuyToSpawn; 
    public int maxTortiToSpawn; 
    public bool canTriggerEvent; 
    public int eventToTrigger;
   // public int max
        #endregion 

  
    [Header("WAVE LOGIC")]
        #region 
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
        #endregion 

    [Header("VISUALS")]
        #region 
    public GameObject waveUI; 
    public TextMeshPro waveText;
    public TextMeshPro enemyCountText;
   // public TextMeshPro waveInfoText;
    public RawImage skullImage; 
        #endregion 

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
                CheckForEnemySpawn();
            }
        }
        else
        {
            TurnOffWave(); 
        }
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
       // SetNextWave();
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
                    SpawnEnemy(currentEnemyToSpawn, choosenSpawnLocation);
                    if (enemiesSpawnedThisWave >= maxEnemiesToSpawnThisWave) totalGroupSize = 0;
                }

                groupSpawnCooldownTimer = groupSpawnCooldownDuration;
            }
        }
    }

  
    void SetEnemyToSpawnType()
    {
        /*
        currentEnemyToSpawn = Random.Range(0, enemyTypesToSpawn.Count - 1);
        if(enemyTypesToSpawn[currentEnemyToSpawn].CompareTag("BasicEnemy"))
        {
            print("Basic Grunt Spawn"); 
            gruntSpawnCount++; 
            if(gruntSpawnCount > )
        
        } 
        
        //currentEnemyToSpawn = (int)EnemyTypes.Grunt;  
        */   

        currentEnemyToSpawn = Random.Range(0, enemySpawnList.Count - 1); 
   
         



    }

    void ChooseEnemyToSpawn()
    {

    }

    void SetSpawnLocation()
    {
        int choosenSpawner = Random.Range(0, enemySpawners.Count);
        choosenSpawnLocation = enemySpawners[choosenSpawner].spawnPoint; 
    }

    void SpawnEnemy(int enemySpawnType, Transform placeToSpawn)
    {
        GameObject enemyToSpawn = enemySpawnList[enemySpawnType];
        GameObject enemy = Instantiate(enemyToSpawn, new Vector3(placeToSpawn.position.x, placeToSpawn.position.y, placeToSpawn.position.z), enemyToSpawn.transform.rotation);     
        enemySpawnList.Remove(enemyToSpawn);  
        enemiesSpawnedThisWave++; 
    }

    void CheckForWave()
    {

        if (waveHasStarted) enemyCountText.text = enemiesLeft.ToString(); 

        if (enemiesSpawnedThisWave >=  maxEnemiesToSpawnThisWave || currentWave == 0) 
        {
            canSpawnNewEnemies = false;
            
            if(enemiesLeft <= 0 || currentWave == 0)
            {
                if (currentWave > 0) waveText.text = "WAVE CLEARED"; 
                waveText.alpha = 1f; 
                waveHasEnded = true;
               // print("current: " + currentWave + "max: " + maxWaves); 
                if (currentWave < maxWaves) DoNextWaveTranstition();           
                else EndWaves(); 
            }
        }
    }


    void SetNextWave()
    {
        currentWave++;
        choosenSpawnData = allWaves[currentWave - 1]; //Select wave to pull data from
         
        //Set enemy types to spawn 
        maxGruntsToSpawn = choosenSpawnData.maxGruntsToSpawn; 
        maxBigGuyToSpawn = choosenSpawnData.maxBigGuyToSpawn; 
        maxTortiToSpawn = choosenSpawnData.maxTortiToSpawn;
        SetEnemySpawnlist();  
      

        //Set spawn data 
        maxEnemiesAllowedInScene = choosenSpawnData.maxEnemiesInScene; //Max enemies allowed in the scene at the same time 
        enemiesLeft = maxEnemiesToSpawnThisWave = maxGruntsToSpawn + maxBigGuyToSpawn + maxTortiToSpawn;
        groupSpawnCooldownDuration = choosenSpawnData.groupSpawnCooldownDuration;
        waveCountdownDuration = choosenSpawnData.waveCountdownDuration;
        //maxEnemiesToSpawnThisWave = choosenSpawnData.maxEnemiesInWave; //Max enemies to spawn in a wave 
         //maxEnemiesToSpawnThisWave = choosenSpawnData.maxEnemiesInWave; //Max enemies to spawn in a wave 
        minGroupSize = choosenSpawnData.minGroupSpawnSize;  //max enimes to spawn in a group 
        maxGroupSize  = choosenSpawnData.maxGroupSpawnSize; //Min enemies to spawn in a group
        currentWaveTitle = choosenSpawnData.WaveTitle;
        canTriggerEvent = choosenSpawnData.canTriggerEvent; 
        eventToTrigger = choosenSpawnData.eventToTrigger; 
        enemiesSpawnedThisWave = 0;

        //Check what we can spawn
        canSpawnEyeballs = choosenSpawnData.canSpawnEyeballs;
        canSpawnLasers = choosenSpawnData.canSpawnLasers;
        canSpawnPilars = choosenSpawnData.canSpawnPilars; 

}

    void SetEnemySpawnlist()
    {
         
        //Grunt enemies to spawn 
        for (int i = 0; i < maxGruntsToSpawn; i++)
        {
            enemySpawnList.Add(allEnemyTypes[0]); 
        }

        //Big Guy enemies to spawn 
        for (int i = 0; i < maxBigGuyToSpawn; i++)
        {
            enemySpawnList.Add(allEnemyTypes[1]); 
        }

        //Big Guy enemies to spawn 
        for (int i = 0; i < maxTortiToSpawn; i++)
        {
            enemySpawnList.Add(allEnemyTypes[2]); 
        }
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
        playerState.AddHealth(100f);
        enemiesLeft = maxEnemiesToSpawnThisWave; 

        if(canTriggerEvent)
        {
            if(eventToTrigger == 7) groupSpawnCooldownTimer = groupSpawnCooldownDuration; 
            eventScript.EndEvent(); 
            eventScript.SetNewEvent(eventToTrigger); 

        
        }

        /*

        //Spawn special objects 
        if(objectSpawnList.Count > 0)
        {
            foreach(GameObject obj in objectSpawnList)
            {
                //obj.SetActive(true); 
                Instantiate(obj, obj.transform.position, obj.transform.rotation); 
                objectsInScene.Add(obj);  
            }

            objectSpawnList.Clear(); 
        }
        */
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



