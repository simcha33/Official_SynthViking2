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
    public Transform hidePosition;

    [Header("Event 1: Íntro")]
    public MMFeedbacks event0Feedback;

    [Header("Event 1: Íntro")]
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

    [Header("Event 5: Tutorial Basic Combat")]
  //  public TextMeshPro basicCombatTutorialText;
    public GameObject basicCombatTutoiral; 



    [Header("MusicClips")]
    // public AudioClip musicClip1; 
    public MMFeedbacks currentSong;

    [Header("Script")]
    public ThirdPerson_PlayerControler playerController;
    public PlayerInputCheck input;
    public CameraHandeler cameraHandelerScript; 

    [Header("Checks")]  
    public bool pauseGame;
    public bool doCountdown; 
    public float eventDuration;
    public float eventTimer;
   

    [Header("Event Camera")]
    public bool blackCamera;
    public CinemachineVirtualCamera currentEventCam;
    public CinemachineVirtualCamera eventCam0;
    public CinemachineVirtualCamera eventCam1;
 
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
      
            doCountdown = true;        
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
            currentEventText = titelCardText; 
         //   eventTimer = currentEventFeedback.GetComponent<MMFeedbackTimescaleModifier>().FeedbackDuration; 
        }
    
        if (currentEventText != null) currentEventText.gameObject.SetActive(true); 
        currentEventFeedback?.PlayFeedbacks(); 
    }

    void DoEventTimer(float duration)
    {
        eventTimer -= Time.deltaTime; 

        if(eventTimer < 0)
        {
            EndEvent(); 
        }
    }

    public void EndEvent()
    {    
        currentEventFeedback?.StopFeedbacks();
        TurnOnUI();
        TurnOnCamera();
     
        if(currentEventFeedback != null) currentEventFeedback = null;
        if (currentEventCam != null) currentEventCam = null;
        if (currentEventText != null) currentEventText.gameObject.SetActive(false);
       
        blackCamera = false; 
        doCountdown = false;
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

    }

    void turnOffCamera()
    {
        blackCamera = true;
        print("Backcam"); 
    }

    void TurnOnCamera()
    {
        print("Backcam NOOO");
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
