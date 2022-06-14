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

    public CinemachineFreeLook defaultCamera;
    public CinemachineFreeLook aimingCamera;
    public CinemachineFreeLook airSmashCamera;
    public CinemachineFreeLook freeFallCamera;
    public CinemachineVirtualCamera mainCam;
    private int highPrio = 10; 
    private int LowPrio = 9; 

    public List<CinemachineFreeLook> cameras = new List<CinemachineFreeLook>(); 

    

    PlayerState playerState; 

  //  public MMFeedbacks airCamera;
  //  public MMFeedbacks defaultCamera; 


    // Start is called before the first frame update
    void Start()
    {
        //mainCam.Priority = 9;    
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCamera(); 

        
    }


    public void CheckForCamera()
    {

        string c;
        if (playerController.isAirSmashing) c = airSmashCamera.name;
        else if (playerController.isFreeFalling && playerController.inAirTime >= playerController.freefallWaitTime) c = freeFallCamera.name; 
        else if (input.dashButtonPressed && playerController.canDash) c = aimingCamera.name;
        else c = defaultCamera.name;
        SwitchPriority(c);
    }

    public void SwitchPriority(string camName)
    {
        
        foreach (CinemachineFreeLook cam in cameras)
        {
            if (cam.name == camName) cam.Priority = highPrio;
            else cam.Priority = LowPrio;
        }
    }


}
