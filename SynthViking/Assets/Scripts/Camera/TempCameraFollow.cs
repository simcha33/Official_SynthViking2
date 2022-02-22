using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    public Transform target;

    public float followSmoothBias;
    ThirdPerson_PlayerControler playerController; 
    public float rotationSmoothBias; 
    public Vector3 offset;
    public float lerpTimer;
    public float lerpDuration;
    //private Vector2 targetTurnRotation;

    void Start()
    {
        playerController = target.GetComponent<ThirdPerson_PlayerControler>(); 
    }

    private void FixedUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position + offset, followSmoothBias);
        transform.position = smoothedPosition;

        transform.rotation = target.rotation; 

        transform.LookAt(target); 

      //  Vector3 targetTurnRotation = Vector3.Lerp(playerController.targetTurnRotation, playerController.moveInput, rotationSmoothBias);
        //transform.rotation = Quaternion.LookRotation(new Vector3(targetTurnRotation.x, 0, targetTurnRotation.y)); //Rotate player towards left stick when only moving 

        //targetTurnRotation = Vector3.Lerp(targetTurnRotation, moveInput, turnLerpTimer);
        //transform.rotation = Quaternion.LookRotation(new Vector3(targetTurnRotation.x, 0, targetTurnRotation.y)); //Rotate player towards left stick when only moving 
    }
}

