using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public float triggerWaitTime;
    public bool hasBeenTriggered;
    public bool canBeRetriggered; 
    private float triggerCooldownTimer;
    public float triggerCooldownDuration;
    public GameObject invisibleWall; 

    [Header ("LaserTrigger")]
    public bool isLaserTrigger;
    public List<LaserTurret> laserTurrets = new List<LaserTurret>(); 


    private enum triggerType
    {
        laserTrigger
    }

    private void Update()
    {
        if(hasBeenTriggered && canBeRetriggered)
        {
            waitForTriggerCooldown(); 
        }
    }

    private string thisTrigger; 

    void Start()
    {
        if (isLaserTrigger)
        {
            thisTrigger = isLaserTrigger.ToString();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(thisTrigger == isLaserTrigger.ToString())
            {
                if (invisibleWall != null) invisibleWall.SetActive(true); 

                foreach(LaserTurret turret in laserTurrets)
                {
                    turret.cooldownDuration = triggerWaitTime;
                    turret.waitForTrigger = false;
                    hasBeenTriggered = true; 
                }
            }
        }
    }

    void waitForTriggerCooldown()
    {
        if(triggerCooldownTimer >= triggerCooldownDuration)
        {
            hasBeenTriggered = false;
            triggerCooldownTimer = 0f; 
        }
    }
}
