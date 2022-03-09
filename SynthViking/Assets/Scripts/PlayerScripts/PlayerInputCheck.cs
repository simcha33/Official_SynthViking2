using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class PlayerInputCheck : MonoBehaviour
{


    //PlayerControls 
    PlayerControls controls;
    ThirdPerson_PlayerControler playerController;


    //checks
    [HideInInspector] public bool dashButtonPressed;
    [HideInInspector] public bool sprintButtonPressed; 
    [HideInInspector] public bool beamButtonPressed;
    [HideInInspector] public bool projectileButtonPressed;
    [HideInInspector] public bool meleeButtonPressed;
    [HideInInspector] public bool parryButtonPressed;
    [HideInInspector] public bool ultimateButtonPressed;
    [HideInInspector] public bool jumpButtonPressed;
    [HideInInspector] public bool airSmashButtonPressed; 
    [HideInInspector] public bool restartSceneButtonPressed; 


    [HideInInspector] public Vector3 moveInput;
    [HideInInspector] public Vector3 aimInput;
    [HideInInspector] public Vector3 rotateInput;

    void Start()
    {
   
    }

    private void Awake()
    {
       controls = new PlayerControls();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        jumpButtonPressed = value.isPressed;
    }

    private void OnDash(InputValue value)
    {
        dashButtonPressed = value.isPressed;
    }

    private void OnSprint(InputValue value)
    {
        sprintButtonPressed = value.isPressed;
    }

    private void OnMeleeAttack(InputValue value)
    {
        meleeButtonPressed = value.isPressed;
    }

    private void OnAirSmash(InputValue value)
    {
        airSmashButtonPressed = value.isPressed; 
    }


    private void OnRestartScene(InputValue value)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }


    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

}
