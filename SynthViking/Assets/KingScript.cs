using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class KingScript : MonoBehaviour
{

    public Transform player;
    public Transform head;

    public bool isSitting;

    public int kingState;
    public int kingFixedState;
    public int kingLateState;

    public bool isProtected;
    public bool avoidPlayer;

    public NavMeshAgent kingAgent;
    public Animator kingAnim;
    public float playerDistance; 

    public enum currentState
    {
        SITTING,
        RUNNING,
    }

    void Start()
    {
        kingAgent = GetComponent<NavMeshAgent>();
        kingAnim = GetComponentInChildren<Animator>();
        kingState = (int)currentState.SITTING; 
    }


    void Update()
    {
        switch (kingState)
        {
            case (int)currentState.SITTING:
                AvoidPlayer();
                break;
        }

        // kingAgent.destination = (player.position - transform.position).normalized * -5f;
      //  kingAgent.destination = player.position; 
    }


    void LateUpdate()
    {
            switch (kingFixedState)
            {
                case (int)currentState.SITTING:
                break;
            }
        
    }

    void FixedUpdate()
    {
            switch (kingLateState)
            {
                case (int)currentState.SITTING:
                    break;
            }

    }


    void Sitting()
    {
       // head.LookAt(player.position); 

    }

    void AvoidPlayer()
    {
        if (!isProtected)
        {
            playerDistance = Vector3.Distance(player.position, transform.position); 

            if (playerDistance <= 10f)
            {
                avoidPlayer = true; 
               // kingAgent.destination = (transform.forward - player.position) * 4f;
            }
            else
            {
                avoidPlayer = false; 
            }

        }

        
    }
}
