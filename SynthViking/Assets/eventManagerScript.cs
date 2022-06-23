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
    private int currentEvent;
    private Vector3 currentPlayerPosition;
    private List<GameObject> delayedObjects = new List<GameObject>(); 
    public List<GameObject> currentObjectsToSpawn = new List<GameObject>(); 


    //Waiting for events
    public float waitForEventTimer;
    public float waitForEventDuration;
    [HideInInspector] public bool waitForEvent;  


    [Header("Components")]
    public CinemachineVirtualCamera mainCam;
    public Light directionalLight;
    public GameObject playerUI;
    public MusicManager musicManagerScript;
    [HideInInspector] public MMFeedbacks currentEventFeedback;
    [HideInInspector] public GameObject currentEnvorinment;
    [HideInInspector] public AudioClip currentMusicclip;
    public TextMeshPro currentEventText;
    public TextMeshPro currentTutorialText;

    public SpawnAreaTrigger currentArenaTrigger;  
    public Transform hidePosition;
    public EnemySpawnManager spawnManager;



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

    [Header("Event 5: Boss intro")]
  //  public TextMeshPro basicCombatTutorialText;
    public MMFeedbacks event5Feedback;
    public GameObject basicCombatTutoiral; 
    public TextMeshPro bossTitleCard;
    public GameObject throneAreaLaser; 
  
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
            
            currentEventFeedback = event1Feedback;
            currentSong = musicFeedback1;
            currentEventCam = eventCam1;
            playerController.transform.position = hidePosition.position; 

            eventDuration = currentEventFeedback.GetComponent<MMFeedbackSound>().FeedbackDuration; //Event will end after a timer 
            eventTimer = eventDuration - .2f; 
      
            //doCountdown = true;        
            musicManagerScript.randomSong = false;
  
            ChangeMusic(); 
            turnOffCamera();
            TurnOffUI();       
         
        }

        //THe big freefall into the planet
        if(currentEvent == 2)
        {
            currentEventFeedback = event2Feedback;
            playerController.transform.position = event2Position.position;
            playerController.inAirTime = playerController.freefallWaitTime;
            currentEnvorinment = environment2;
            barrackEnvironment.SetActive(false); 
            currentEnvorinment.SetActive(true);
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
            waitForEvent = true; 
            waitForEventDuration = 1.4f; 
            eventTimer = eventDuration = 5f; 
            currentEventFeedback = event5Feedback; 
            currentEventCam = event5Cam; 

        
            
        }

        //First comba arena triggered 
        if(currentEvent == 6)
        {
            currentArenaTrigger = combatArena1Trigger;  
           
        }

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
        waitForEventTimer = 0;
        eventTimer = 0;


        //Transiion from intro dialog and free fall
        if (currentEvent == 1)
        {
            SetNewEvent(2);

        }
        else if(currentEvent == 2)
        {
            currentEnvorinment.SetActive(false); 
        }
        else if (currentEvent == 5)
        {
            SetNewEvent(6); 
            throneAreaLaser.SetActive(true);

        }

    }

    void turnOffCamera()
    {
        blackCamera = true;
        print("Backcam"); 
    }

    void TurnOnCamera()
    {
    
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
        musicManagerScript.ChooseNextSong(); 
 
    }



   

    

  

  
}
