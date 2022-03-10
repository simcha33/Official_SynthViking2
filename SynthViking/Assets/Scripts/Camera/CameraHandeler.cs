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
    

    PlayerState playerState; 

  //  public MMFeedbacks airCamera;
  //  public MMFeedbacks defaultCamera; 


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SwitchPriority(); 
    }


    public void SwitchPriority()
    {

        if (input.dashButtonPressed && playerController.canDash)
        {
            aimingCamera.Priority = 10;
            defaultCamera.Priority = 9;
        }
        else
        {
            defaultCamera.Priority = 10;
            aimingCamera.Priority = 9;                  
        }  
    }


}
