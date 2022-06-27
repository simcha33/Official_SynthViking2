using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using TMPro; 
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{

     public ThirdPerson_PlayerControler playerController;
    public eventManagerScript eventManager;
 public PlayerState playerState;
  public PlayerInputCheck _playerInputCheck; 
    public bool gameIsPaused;
    public float hapticDuration = 0f; 
    public float curLowFreq, cureHighFreq; 
    private Gamepad gamepad;
    private static GameManager instance;
    public Transform respawnPoint;
    public  int eventToStartAt;
    public bool hasRespawned; 



    // Start is called before the first frame update
    void Awake()
    {

        /*

        if (instance == null)
        {
            print("instance");
            instance = this;

        }
        else
        {
            Destroy(gameObject);
            print("nono instance");
            SetRespawn(); 


        }

        DontDestroyOnLoad(instance);
        */

        gamepad = Gamepad.current;
        gamepad.SetMotorSpeeds(0, 0);

    }

    private void Start()
    {

    }

    void SetRespawn()
    {

      
        eventManager.eventToStartAt = eventToStartAt;
        playerController.transform.position = respawnPoint.position;
      

  
    }

    // Update is called once per frame
    void Update()
    {
        if (gamepad != null) if (hapticDuration > 0) DoHaptics(hapticDuration, curLowFreq, cureHighFreq);

    }
    public void ResetScene()
    {

        hasRespawned = true; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    void LoadNextScene()
    {

    }

    public void DoHaptics(float duration, float lowFreq, float highFreq)
    {
        if (gamepad != null)
        {
            curLowFreq = lowFreq;
            cureHighFreq = highFreq;
            hapticDuration = duration;
            hapticDuration -= Time.deltaTime;
            gamepad.SetMotorSpeeds(lowFreq, highFreq);

            if (hapticDuration <= 0) InputSystem.ResetHaptics();
        }
    }




}
