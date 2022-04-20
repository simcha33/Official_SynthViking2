using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{

    public ThirdPerson_PlayerControler playerController;
    public PlayerState playerState;
    public bool gameIsPaused; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScene()
    {      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    void LoadNextScene()
    {

    }
}
