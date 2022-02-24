using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class CameraHandeler : MonoBehaviour
{
    public List <GameObject> playerCams = new List<GameObject>(); 
    private int currentCam; 
    ThirdPerson_PlayerControler playerController; 
    PlayerState playerState; 

    public MMFeedbacks airCamera;
    public MMFeedbacks defaultCamera; 


    // Start is called before the first frame update
    void Start()
    {
      //  defaultCamera?.PlayFeedbacks(); 
    }

    // Update is called once per frame
    void Update()
    {
       // if(playerController.isInAir) airCamera?.PlayFeedbacks(); 
      //  else if(playerController.isGrounded || playerController.isLanding) defaultCamera?.PlayFeedbacks(); 
    }
}
