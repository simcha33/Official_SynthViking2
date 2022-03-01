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
    public GameObject activeCameras; 
    public CinemachineStateDrivenCamera camStates;
    public CinemachineBlenderSettings defaultBlends;
    public CinemachineBlenderSettings aimingBlends;


    PlayerState playerState; 

  //  public MMFeedbacks airCamera;
  //  public MMFeedbacks defaultCamera; 


    // Start is called before the first frame update
    void Start()
    {
      //  defaultCamera?.PlayFeedbacks(); 
    }

    // Update is called once per frame
    void Update()
    {
        activeCameras = camStates.VirtualCameraGameObject; 
        if (playerController.isChargingDash)
        {
            //camStates.m_CustomBlends; 
        }
    }
}
