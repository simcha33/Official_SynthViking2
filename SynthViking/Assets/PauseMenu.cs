using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using MoreMountains.Feedbacks; 

public class PauseMenu : MonoBehaviour
{

    public PlayerInputCheck inputCheck; 
    public GameObject pauseMenuCanvas; 

    public MMTimeManager timeMod; 
    
    public bool gameIsPaused;
    public bool wait; 


    // Update is called once per frame

    void Start(){
        gameIsPaused = false; 
    }
    void Update()
    {
            if(inputCheck.pauseButtonPressed)
            {
                wait = true; 
               
            }
            if(!inputCheck.pauseButtonPressed && wait){
                    wait =false; 
                  CheckForPauseInput(); 
                

            }
    }

    void CheckForPauseInput()
    {
    
      
        if(!gameIsPaused)
        {
            PauseGame(); 
        }
        else
        {
            ResumeGame(); 
        }
        
    }

    void PauseGame()
    {
        gameIsPaused = true ; 
       // Time.timeScale = 0f; 

       timeMod.SetTimescaleTo(0f); 
         print("DO PAUSEE"); 
        pauseMenuCanvas.SetActive(true); 

    }

    void ResumeGame()
    {

        gameIsPaused = false; 
           timeMod.SetTimescaleTo(1f); 
       // Time.timeScale = 1f; 
        pauseMenuCanvas.SetActive(false); 

    }
}
