using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{

    public string selectedLevel;
    public PlayerInputCheck inputCheck; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForControllerInput();
    }

    void CheckForControllerInput()
    {
        if (inputCheck.jumpButtonPressed) StartGame();
        if (inputCheck.airSmashButtonPressed) QuitGame(); 
    }

    public void StartGame()
    {
        SceneManager.LoadScene(selectedLevel); 
    }

    public void OpenOptions()
    {

    }

    public void closeOptions(){

    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
