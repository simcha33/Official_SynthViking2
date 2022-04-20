using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using MoreMountains.Feedbacks; 

public class PauseMenu : MonoBehaviour
{

    public PlayerInputCheck inputCheck; 
    public GameObject pauseMenuCanvas;
    public GameManager mainGameManager;
    public string MainMenu; 

    public MMTimeManager timeMod; 
    
    public bool gameIsPaused;
    public bool wait;
    public Vector3 oldPos; 


    // Update is called once per frame

    void Start(){
        gameIsPaused = false; 
    }
    void Update()
    {
        if (mainGameManager.playerState.playerIsDead)
        {
            PauseGame(); 
        }
        else if(inputCheck.pauseButtonPressed)
        {
            wait = true;             
        }
        if(!inputCheck.pauseButtonPressed && wait)
        {
            wait = false; 
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
        gameIsPaused = true;
        mainGameManager.gameIsPaused = true;
        timeMod.SetTimescaleTo(0f);
        oldPos = mainGameManager.playerController.transform.position; 
        pauseMenuCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!mainGameManager.playerState.playerIsDead)
        {
            gameIsPaused = false;
            mainGameManager.gameIsPaused = false;
            mainGameManager.playerController.transform.position = oldPos;
            timeMod.SetTimescaleTo(1f);
            pauseMenuCanvas.SetActive(false);
        }
    }

    public void ResetScene()
    {
        mainGameManager.ResetScene(); 
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void OpenSettings()
    {

    }
}
