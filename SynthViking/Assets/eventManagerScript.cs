using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Cinemachine;
using UnityEngine.UI;
using TMPro;


public class eventManagerScript : MonoBehaviour
{
    public bool startWithEvent;
    public int eventToStartAt; 
    //public int totalEvents;
    public int currentEvent;
    private Vector3 currentPlayerPosition;
    private List<GameObject> delayedObjects = new List<GameObject>(); 
    public List<GameObject> currentObjectsToSpawn = new List<GameObject>(); 


    //Waiting for events
    public float waitForEventTimer;
    public float waitForEventDuration;
    public bool freezeTheGame;
    public bool gameIsFroozen;
    public bool checkForCondition;
    public bool eventWasSkipped;
    private float buttonHoldDuration = 1.1f;
    private float buttonHoldTimer;
    public Image buttonHoldImage;
    [HideInInspector] public bool waitForEvent;  


    [Header("Components")]
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera blackCam; 
    public Light directionalLight;
    public GameObject playerUI;
    public MusicManager musicManagerScript;
    public MMFeedbacks currentEventFeedback;
    [HideInInspector] public GameObject currentEnvorinment;
     public AudioClip currentMusicclip;
    public TextMeshPro currentEventText;
    public TextMeshPro currentTutorialText;

    public SpawnAreaTrigger currentArenaTrigger;  
    public Transform hidePosition;
    public EnemySpawnManager spawnManager;
    public GameManager _gameManager;
    public Transform respawnPoint;
    public TutorialManager _tutorialManager;
    public MusicManager2 _musicManager2; 



    [Header("Event 1: �ntro")]
    public Transform event1Position;
    public GameObject environment1;
  //  public float introDuration;
    public MMFeedbacks musicFeedback1;
    public MMFeedbacks event1Feedback;
  
    [Header("Event 2: The drop")]
    public Transform event2Position;
    public GameObject environment2;
    public MMFeedbacks event2Feedback;

    [Header("Event 3: PlanetSwap")]
    public GameObject outsidePlanet;
    public GameObject insidePlanet;
    public GameObject barrackEnvironment;

    [Header("Event 4: The title card")]
    public MMFeedbacks event4Feedback;
    public TextMeshPro titelCardText;
    public GameObject event4Trigger;

    [Header("Event 5: Boss intro")]
  //  public TextMeshPro basicCombatTutorialText;
    public MMFeedbacks event5Feedback;
    public GameObject basicCombatTutoiral; 
    public TextMeshPro bossTitleCard;
    public GameObject throneAreaLaser;
    public GameObject event5Trigger; 
  
    [Header("Event 6: First combat encounter")]
    public SpawnAreaTrigger combatArena1Trigger;  

    [Header("Event 7: First combat encounter")]
  //  public TextMeshPro basicCombatTutorialText;
    public MMFeedbacks event7Feedback;
    public TextMeshPro bigGuyTitleCard;
    public GameObject bigGuy; 



    [Header("MusicClips")]
    // public AudioClip musicClip1; 
    public MMFeedbacks currentSong;

    [Header("Script")]
    public ThirdPerson_PlayerControler playerController;
    public PlayerInputCheck input;
    public CameraHandeler cameraHandelerScript; 

    [Header("Checks")]  
    public bool pauseGame;
  //  public bool doCountdown; 


    public float eventDuration;
    public float eventTimer;
   

    [Header("Event Camera")]
    public bool blackCamera;
    public CinemachineVirtualCamera currentEventCam;
    public CinemachineVirtualCamera eventCam0;
    public CinemachineVirtualCamera eventCam1;
     public CinemachineVirtualCamera event5Cam;
    public CinemachineVirtualCamera event7Cam;
 
    public List<CinemachineVirtualCamera> eventCameras = new List<CinemachineVirtualCamera>(); 


    void Start()
    {

       //currentEvent = 1;       
       eventTimer = 0f;

        if (startWithEvent)
        {
            currentEvent = eventToStartAt;
            SetNewEvent(eventToStartAt);
        }            
    }

    // Update is called once per frame
    void Update()
    {
        if (eventTimer > 0) DoEventTimer(eventDuration);
        if(waitForEvent) WaitForEventTrigger();
        if (freezeTheGame) FreezeGame(); 
        if(!freezeTheGame && gameIsFroozen)
        {
            UnfreezeGame(); 
        }
        if (checkForCondition) CheckForEventCondition(); 

    }

    public void SetNewEvent(int eventInt)
    {
        currentEvent = eventInt;

        if (currentEvent == 0)
        {
            /*
            currentEventFeedback = event0Feedback;
            currentSong = musicFeedback1;
            currentEventCam = eventCam0;
            playerController.transform.position = hidePosition.position;
             eventDuration = currentEventFeedback.GetComponent<MMFeedbackSound>().FeedbackDuration - .5f; //Event will end after a timer 
            eventTimer = 45f;
            doCountdown = true;

            musicManagerScript.randomSong = false;
            ChangeMusic();
            turnOffCamera();
            TurnOffUI();
            */


        }

        //Starting dialog 
        if (currentEvent == 1)
        {
            checkForCondition = true; 
            currentEventFeedback = event1Feedback;
            currentSong = musicFeedback1;
            currentEventCam = eventCam1;
            playerController.transform.position = hidePosition.position; 

            eventDuration = currentEventFeedback.GetComponent<MMFeedbackSound>().FeedbackDuration; //Event will end after a timer 
            eventTimer = eventDuration - .2f; 
      
            //doCountdown = true;        
           // musicManagerScript.randomSong = false;
  
          //  ChangeMusic(); 
            turnOffCamera();
            //TurnOffUI();       
         
        }

        //THe big freefall into the planet
        if(currentEvent == 2)
        {
       
            currentEventFeedback = event2Feedback;
            playerController.transform.position = event2Position.position;
            playerController.inAirTime = playerController.freefallWaitTime;
            environment2.SetActive(true);
            _musicManager2.ChooseNewSong(false);
            // currentEnvorinment = environment2;
            //  barrackEnvironment.SetActive(false); 
            //   currentEnvorinment.SetActive(true);
            // outsidePlanet.SetActive(true);
            //  insidePlanet.SetActive(false);

        }

        //Planet swap
        if (currentEvent == 3)
        {
            //outsidePlanet.SetActive(false);
          //insidePlanet.SetActive(true);
            currentEnvorinment = barrackEnvironment;
            barrackEnvironment.SetActive(true); 
        }

        //Title Card
        if (currentEvent == 4)
        {
            currentEventFeedback = event4Feedback;
        //    currentEventText = titelCardText; 
         //   eventTimer = currentEventFeedback.GetComponent<MMFeedbackTimescaleModifier>().FeedbackDuration; 
        }
    
        //Boss intro title card
        if(currentEvent == 5)
        {
            freezeTheGame = true; 
            waitForEvent = true; 
            waitForEventDuration = .5f; 
            eventTimer = eventDuration = 5f; 
            currentEventFeedback = event5Feedback; 
            currentEventCam = event5Cam;            
        }

        //First comba arena triggered / Also respawn point
        if(currentEvent == 6)
        {
            TurnOnUI();
            environment2.SetActive(false);
            throneAreaLaser.SetActive(true);
            event1Feedback?.StopFeedbacks();
            event4Trigger.SetActive(false);
            event5Trigger.SetActive(false);
            _gameManager.eventToStartAt = currentEvent;
            _gameManager.respawnPoint = respawnPoint;
            currentArenaTrigger = combatArena1Trigger;
            _tutorialManager.StartTutorial(); 

        }

        //Big guy intro
        if(currentEvent == 7)
        {
        
            waitForEvent = true; 
            waitForEventDuration = 1.4f;      
        }

       

        if (currentEventText != null && !delayedObjects.Contains(currentEventText.gameObject)) currentEventText.gameObject.SetActive(true); 
        if(currentEventFeedback != null && !delayedObjects.Contains(currentEventFeedback.gameObject)) currentEventFeedback?.PlayFeedbacks(); 
        if(currentArenaTrigger != null){ currentArenaTrigger.SetNewSpawnArea();  
       
        }
    }

    void DoEventTimer(float duration)
    {
        eventTimer -= Time.deltaTime; 

        if(eventTimer < 0)
        {
            EndEvent(); 
        }
    }

    void CheckForEventCondition()
    {
        if(currentEvent == 1)
        {
            if (input.airSmashButtonPressed)
            {
                buttonHoldTimer += Time.deltaTime;
                buttonHoldImage.fillAmount = buttonHoldTimer / buttonHoldDuration;
            }
            else buttonHoldTimer = buttonHoldImage.fillAmount = 0f;
        

        

            if (buttonHoldTimer >= buttonHoldDuration && !_gameManager.gameIsPaused)
            {
                
                eventWasSkipped = true;     
                
                EndEvent();
                currentEvent = 6;


                event1Feedback?.StopFeedbacks(); 
                barrackEnvironment.SetActive(true);
                combatArena1Trigger.waveCountdownDuration = 5f;
                SetNewEvent(currentEvent);

                //TurnOffUI();
                playerController.isFreeFalling = true;
                playerController.inAirTime = playerController.freefallWaitTime; 
                playerController.transform.position = respawnPoint.position; 
                eventWasSkipped = false;
                currentSong = musicFeedback1;
                // ChangeMusic();
                _musicManager2.ChooseNewSong(true); 
              
                buttonHoldTimer = 0f; 
              
            }
        }
    }

    public void SpawnObjects()
    {
        foreach(GameObject obj in currentObjectsToSpawn)
        {
            obj.SetActive(true); 
        }

    
    }

    void WaitForEventTrigger()
    {
        waitForEventTimer += Time.deltaTime;
        if(waitForEventTimer >= waitForEventDuration)
        { 
            if(currentEvent != 7) waitForEvent = false; 
            waitForEventTimer = 0; 

            if(delayedObjects.Count > 0)
            {
                foreach(GameObject obj in delayedObjects)
                { 
                    obj.SetActive(true); 
                }

                delayedObjects.Clear(); 
            }

            if(currentEvent == 7 && spawnManager.spawnedAliveEnemies.Count > 0)
            {
                eventTimer = eventDuration = 4.5f; 
                currentEventFeedback = event7Feedback; 
                currentEventFeedback?.PlayFeedbacks();             
                bigGuy = spawnManager.spawnedAliveEnemies[0].gameObject.GetComponent<BasicEnemyScript>().lookAt; 
                event7Cam.Follow = bigGuy.transform;
                event7Cam.LookAt = bigGuy.transform;
                currentEventCam = event7Cam;
                freezeTheGame = true;
                waitForEvent = false;
               
            }
        }
    }

    public void EndEvent()
    {

 
        currentEventFeedback?.StopFeedbacks();    
        TurnOnUI();


        TurnOnCamera();
        delayedObjects.Clear(); 
      
        currentObjectsToSpawn.Clear(); 
        if(currentEventFeedback != null) currentEventFeedback = null;
        if (currentEventCam != null) currentEventCam = null;
        if (currentEventText != null) currentEventText.gameObject.SetActive(false);
        if(currentArenaTrigger != null) currentArenaTrigger = null; 
       
        blackCamera = false; 
      //  doCountdown = false;
        waitForEvent = false;
        freezeTheGame = false;
        checkForCondition = false;
        waitForEventTimer = 0;
        eventTimer = 0;


        //Transiion from intro dialog and free fall
        if (currentEvent == 1)
        {
            if(!eventWasSkipped) SetNewEvent(2);

        }
        else if(currentEvent == 2)
        {
            //currentEnvorinment.SetActive(false); 
        }
        else if (currentEvent == 5)
        {
            if(!eventWasSkipped) SetNewEvent(6); 
            throneAreaLaser.SetActive(true);

        }

    }

    void turnOffCamera()
    {
        blackCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false); 
        blackCamera = true;
    }

    void TurnOnCamera()
    {
        blackCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        blackCamera = false; 
    }



    void TurnOffUI()
    {
        playerUI.SetActive(false); 

    }

    void TurnOnUI()
    {
        playerUI.SetActive(true); 
    }

    void ChangeMusic()
    {
     //   musicManagerScript.ChooseNextSong(); 
 
    }

    void FreezeGame()
    {
        gameIsFroozen = true;
        playerController.playerRb.velocity = new Vector3(0, 0, 0);
        playerController.eventPausing = true;

        foreach (GameObject enemy in spawnManager.spawnedAliveEnemies)
        {
            if (enemy.GetComponent<BasicEnemyScript>() != null)
            {
                BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>();
                enemyScript.enemyAgent.speed = .5f;
                enemyScript.canAttack = false; 
            }
        }


        // playerController.playerRb.

    }

    public void UnfreezeGame()
    {
        gameIsFroozen = false;
        freezeTheGame = false; 

        playerController.playerState.canBeHit = true;
        playerController.eventPausing = false;

        foreach (GameObject enemy in spawnManager.spawnedAliveEnemies)
        {
            if (enemy.GetComponent<BasicEnemyScript>() != null)
            {
                BasicEnemyScript enemyScript = enemy.GetComponent<BasicEnemyScript>();
                enemyScript.enemyAgent.speed = enemyScript.enemyAgent.speed = enemyScript.runSpeed; 
                //   enemyScript.canAttack = true; 
            }
        }

     


    }









}
