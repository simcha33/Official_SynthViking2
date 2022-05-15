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
    public PlayerState playerState;
    public bool gameIsPaused;
    public float hapticDuration = 0f; 
    public float curLowFreq, cureHighFreq; 
    private Gamepad gamepad; 

    // Start is called before the first frame update
    void Awake()
    {
          gamepad = Gamepad.current;
         // hapticDuration = 2f; 
          //DoHaptics(2f, .1f ,.3f); 
    }

    // Update is called once per frame
    void Update()
    {
        if (gamepad != null)
        {
            if (hapticDuration > 0) DoHaptics(hapticDuration, curLowFreq, cureHighFreq);
        }
        else
        {
            gamepad = Gamepad.current;
        }
    }

    public void ResetScene()
    {      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    void LoadNextScene()
    {

    }

    public void DoHaptics(float duration, float lowFreq, float highFreq)
    {
        curLowFreq = lowFreq; 
        cureHighFreq = highFreq; 
        hapticDuration = duration; 
        hapticDuration -= Time.deltaTime; 
        gamepad.SetMotorSpeeds(lowFreq, highFreq);

        if(hapticDuration <= 0) InputSystem.ResetHaptics(); 
    }




}
