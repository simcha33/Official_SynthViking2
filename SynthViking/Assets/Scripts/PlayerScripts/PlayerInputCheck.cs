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
    [HideInInspector] public bool heavyAttackButtonPressed;
    [HideInInspector] public bool lightAttackButtonPressed;
    [HideInInspector] public bool blockButtonPressed;
    [HideInInspector] public bool ultimateButtonPressed;
    [HideInInspector] public bool jumpButtonPressed;
    [HideInInspector] public bool airSmashButtonPressed; 
    [HideInInspector] public bool restartSceneButtonPressed;
    [HideInInspector] public bool slowMoButtonPressed;
    [HideInInspector] public bool dPadLeftPressed;
    [HideInInspector] public bool dPadRightPressed;


    [HideInInspector] public bool pauseButtonPressed; 


    [HideInInspector] public Vector3 moveInput;
    [HideInInspector] public Vector3 aimInput;
    [HideInInspector] public Vector3 rotateInput;
    public Gamepad gamepad; 

    void Start()
    {
        gamepad = Gamepad.current; 
    }

    /*
    private void Update()
    {
        if(gamepad == null)
        {
            print("Not in input check"); 
        }
    }
    */
    private void Awake()
    {
       controls = new PlayerControls();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    /*
    private void OnLook(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    */


    private void OnJump(InputValue value)
    {
        jumpButtonPressed = value.isPressed;
    }

    private void OnBlock(InputValue value)
    {
        blockButtonPressed = value.isPressed; 
    }


    private void OnDash(InputValue value)
    {
        dashButtonPressed = value.isPressed;
    }

    private void OnSprint(InputValue value)
    {
        sprintButtonPressed = value.isPressed;
    }

    private void OnSlowMo(InputValue value)
    {
        slowMoButtonPressed = value.isPressed;
    }

    private void OnMeleeAttack(InputValue value)
    {
        heavyAttackButtonPressed = value.isPressed;
    }

    private void OnLightAttack(InputValue value)
    {
        lightAttackButtonPressed = value.isPressed; 
    }


    private void OnAirSmash(InputValue value)
    {
        airSmashButtonPressed = value.isPressed; 
    }

     private void OnPauseGame(InputValue value)
    {
        pauseButtonPressed = value.isPressed; 
    }

    private void OnDpadLeft(InputValue value)
    {
        dPadLeftPressed = value.isPressed;
    }
    
    private void OnDpadRight(InputValue value)
    {
        dPadRightPressed = value.isPressed;
    }





    private void OnRestartScene(InputValue value)
    {
        restartSceneButtonPressed = value.isPressed; 
    }


    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
     //   controls.Gameplay.Disable();
    }

}
