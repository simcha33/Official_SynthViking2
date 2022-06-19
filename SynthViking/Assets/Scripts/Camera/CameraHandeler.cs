using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Cinemachine; 

public class CameraHandeler : MonoBehaviour
{
  //  public List <GameObject> playerCams = new List<GameObject>(); 
  //  private int currentCam; 
    public ThirdPerson_PlayerControler playerController;
    public PlayerInputCheck input;
    public eventManagerScript eventScript; 

    public CinemachineFreeLook defaultCamera;
    public CinemachineFreeLook aimingCamera;
    public CinemachineFreeLook airSmashCamera;
    public CinemachineFreeLook freeFallCamera;
    //   public CinemachineVirtualCamera mainCam;
    public Camera mainCam; 
   
    private int highPrio = 10; 
    private int LowPrio = 9;

    public float manualCamUsedTimer; 
    public float autoCamEnabledDuration; 

    public List<CinemachineFreeLook> playerCameras = new List<CinemachineFreeLook>();
    public List<CinemachineVirtualCamera> eventCameras = new List<CinemachineVirtualCamera>();



    PlayerState playerState; 

  //  public MMFeedbacks airCamera;
  //  public MMFeedbacks defaultCamera; 


    // Start is called before the first frame update
    void Start()
    {
        //mainCam.Priority = 9;  
        eventCameras = eventScript.eventCameras; 
    }

    // Update is called once per frame
    void Update()
    {

        CheckForCamera(); 
    }


    public void CheckForCamera()
    {

        string c = "";

        if (eventScript.currentEventCam == null)
        {
            if (mainCam.enabled == false) mainCam.enabled = true; 
            if (playerController.isAirSmashing) c = airSmashCamera.name;
            else if (playerController.inAirTime >= playerController.freefallWaitTime && playerController.isFreeFalling) c = freeFallCamera.name;
            else if (input.dashButtonPressed && playerController.canDash) c = aimingCamera.name;
            else c = defaultCamera.name;

          //  print(c);
      
        }
        else if(eventScript.currentEventCam != null)
        {

            if (!eventScript.blackCamera)
            {
                if (mainCam.enabled == false) mainCam.enabled = true; 
                c = eventScript.currentEventCam.name;
            }
            else mainCam.enabled = false;
            
          //  else mainCam.
        //    else mainCam.
          //  else mainCam.gameObject.SetActive(false); 
        }

        SwitchPriority(c);
    }

    public void SwitchPriority(string camName)
    {
        if (eventScript.currentEventCam == null)
        {
            //Set correct player camera 
            foreach (CinemachineFreeLook cam in playerCameras)
            {
                if (cam.name == camName) cam.Priority = highPrio;
                else cam.Priority = LowPrio;
            }

            //Set player camera's to low priority
            foreach (CinemachineVirtualCamera cam in eventCameras)
            {
                if (cam.name == camName) cam.Priority = highPrio;
                else cam.Priority = LowPrio;
            }

  
        }
        else if (eventScript.currentEventCam != null)
        {
            //Set correct event camera 
            foreach (CinemachineVirtualCamera cam in eventCameras)
            {
                if (cam.name == camName) cam.Priority = highPrio;
                else cam.Priority = LowPrio;
            }

            //Set player camera's to low priority 
            foreach (CinemachineFreeLook cam in playerCameras)
            {
                if (cam.name == camName) cam.Priority = highPrio;
                else cam.Priority = LowPrio;
            }
        }
    }
}
