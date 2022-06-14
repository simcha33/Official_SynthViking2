using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{

   // public Transform turretBarrel;
    public Transform turretBase; 
    public GameObject laserObject;
    public GameObject laserOrb; 

    public float activeDuration;
    private float activeTimer;
    private bool isActivated;
    private bool canActivate;


    [Header("Trigger")]
    public bool waitForTrigger; 
    public bool stayOn;
    public TrapTrigger trapTrigger; 

    public float cooldownDuration;
    public float randomDuration;  
    private float cooldownTimer;

    //Swiffle
    public bool canSwiffle;

    [Header ("xSwiffle")]
    public bool xSwiffle;
    public float xSpeed;
    public float xAmount;
    public float xMaxSwiffle;
    private float xDefault;

    [Header("ySwiffle")]
    public bool ySwiffle;
    public float ySpeed;
    public float yAmount;
    public float yMaxSwiffle;
    private float yDefault; 

    [Header("zSwiffle")]
    public bool zSwiffle;
    public float zSpeed;
    public float zAmount;
    public float zMaxSwiffle;
    private float zDefault;





    private void Start()
    {
    
        ResetLaser();
        xDefault = turretBase.eulerAngles.x;
        yDefault = turretBase.eulerAngles.y;
        zDefault = turretBase.eulerAngles.z;

        if (waitForTrigger)
        {
            trapTrigger.isLaserTrigger = true; 
            trapTrigger.laserTurrets.Add(GetComponent<LaserTurret>()); 
        }
    }

    private void Update()
    {
        if (!canActivate) CheckForCooldown();
        else DoLaser();

        if (xSwiffle || ySwiffle || zSwiffle) canSwiffle = true;
        else canSwiffle = false; 
    }

    void CheckForCooldown()
    {
        if (cooldownTimer < cooldownDuration && !waitForTrigger) cooldownTimer += Time.deltaTime;
        else if(!waitForTrigger) EnableLaser();

        if (cooldownTimer > cooldownDuration - 1f) laserOrb.SetActive(true); 
    }

    void ResetLaser()
    {
        laserObject.SetActive(false);
        laserOrb.SetActive(false);
        cooldownTimer = Random.Range(0 - randomDuration, 0 + randomDuration);
        activeTimer = activeDuration;

        isActivated = false;
        canActivate = false;
    }

    void EnableLaser()
    {
        canActivate = true;
        laserObject.SetActive(true);
    }

    void DoLaser()
    {
        isActivated = true;
        activeTimer -= Time.deltaTime;
        if (canSwiffle) DoSwiffle();
        if (activeTimer <= 0 && !stayOn) ResetLaser();
    }

    void DoSwiffle()
    {
        if (xSwiffle)  xAmount += Time.deltaTime * xSpeed;
        else xAmount = turretBase.eulerAngles.x;

        if (ySwiffle)  yAmount += Time.deltaTime * ySpeed;
        else yAmount = turretBase.eulerAngles.y;

        if (zSwiffle)  zAmount += Time.deltaTime * zSpeed;
        else zAmount = turretBase.eulerAngles.z; 

        if (xSwiffle && (xAmount - xDefault >= xMaxSwiffle || xAmount - xDefault <= -xMaxSwiffle)) xSpeed *= -1;
        if (ySwiffle && (yAmount - yDefault >= yMaxSwiffle || yAmount - yDefault <= -yMaxSwiffle)) ySpeed *= -1;
        if (zSwiffle && (zAmount - zDefault >= zMaxSwiffle || zAmount - zDefault <= -zMaxSwiffle)) zSpeed *= -1;

        turretBase.eulerAngles = new Vector3(xAmount, yAmount, zAmount); 
    }

    void DamageTarget()
    {

    }
}
