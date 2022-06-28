using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using MoreMountains.Feedbacks;
using TMPro; 


public class PauseMenu : MonoBehaviour
{

    public PlayerInputCheck inputCheck; 
    public GameObject pauseMenuCanvas;
    public GameObject gameOverMenuCanvas; 
    public GameManager mainGameManager;
    public TextMeshPro gameOverText;
    public TutorialManager _tutorialManager;
    public string MainMenu; 

    public MMTimeManager timeMod;
    public MMFeedbacks pauseFeedback; 
    
    public bool gameIsPaused;
    public bool wait;
    public Vector3 oldPos; 


    // Update is called once per frame

    void Start()
    {
        gameIsPaused = false;
        gameOverText.alpha = 0f; 
    }
    void Update()
    {
        if (mainGameManager.playerState.playerIsDead)
        {
            GameOverScreen();
            gameIsPaused = true; 
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

    void GameOverScreen()
    {
     //   pauseFeedback?.PlayFeedbacks();
        mainGameManager.playerController.enabled = false;
        timeMod.SetTimescaleTo(0f);
        gameOverMenuCanvas.SetActive(true);
        //gameOverText.alpha += Time.deltaTime * .5f;
        gameOverText.alpha = 1f; 
    }


    void PauseGame()
    {
        pauseFeedback?.PlayFeedbacks();
        mainGameManager.playerController.enabled = false; 
        gameIsPaused = true;
        mainGameManager.gameIsPaused = true;
        timeMod.SetTimescaleTo(0f);
        oldPos = mainGameManager.playerController.transform.position;
        pauseMenuCanvas.SetActive(true);
        _tutorialManager.StartTutorial(); 
    }

    public void ResumeGame()
    {
        if (!mainGameManager.playerState.playerIsDead)
        {
 
            gameIsPaused = false;
            mainGameManager.gameIsPaused = false;

            timeMod.SetTimescaleTo(1f);
            pauseMenuCanvas.SetActive(false);
            mainGameManager.playerController.enabled = true;
            mainGameManager.playerController.playerRb.velocity = new Vector3(0,0,0);
            mainGameManager.playerController.transform.position = oldPos;
            _tutorialManager.StopTutorial(); 
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
