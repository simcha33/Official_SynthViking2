using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI;
using DG.Tweening;

public class SoulPilar : MonoBehaviour
{

   
    public float soulCheckRadius; 
    public float soulsNeeded;
    public float currentSouls; 
    private float stayTimer;
    public float stayDuration; 
     public bool hasSpawned; 

     public bool deSpawn;
    public float soulSuckSpeed;  
    private bool pilarIsFull; 

    [Header("Visuals")]
    public GameObject fullPilarEffect; 


    [Header("Components")]
    public SphereCollider checkCol;
    public GameObject soulRadiusObject; 
    private SoulPilar thisScript;
    private ThirdPerson_PlayerControler playerController;
    public Animator pilarAnim;
    public RewardPilarManager pilarManager; 
  


    [Header("Health Pilar")]
    public bool isHealthPilar;
    public float healthAddAmount; 


    [Header("Trap pilar")]
    public bool isTrapPilar; 
    public List<GameObject> trapsToActivate = new List<GameObject>(); 

    void Start()
    {
       // ResetPilar
        currentSouls = 0; 
        stayTimer = 0;
    
        pilarManager = GameObject.Find("PilarManager").GetComponent<RewardPilarManager>(); 
        thisScript = gameObject.GetComponent<SoulPilar>(); 
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
        checkCol = gameObject.GetComponent<SphereCollider>(); 
        soulCheckRadius = checkCol.radius;
        gameObject.SetActive(false); 
    }

    void Update()
    {
        if(hasSpawned) StayCountdown();
        if(deSpawn) RemovePilar(); 
    }

    void StayCountdown()
    {
        if(stayTimer == 0)
        { 
            pilarAnim.SetTrigger("Spawn");
           // gameObject.SetActive(true); 
            
        }
        stayTimer += Time.deltaTime;
        if(stayTimer >= stayDuration)
        {
            RemovePilar(); 
        } 
    }

    void RemovePilar()
    {
        if(!deSpawn)
        { 
            pilarAnim.SetTrigger("Despawn"); 
            deSpawn = true; 
            stayTimer = stayDuration - 3f;
        }

        if(stayTimer >= stayDuration)
        {
            ResetPilar(); 
        }
     
  
    }

    void ResetPilar()
    {
        if(!pilarIsFull) pilarManager.SetText("PILAR HAS LEFT"); 
        gameObject.SetActive(false); 
        pilarIsFull = false; 
        deSpawn = false; 
        currentSouls = 0f;
        hasSpawned = false;
        stayTimer = 0f; 
        pilarManager.currentPilarsInScene--;
    }

  

    private void OnTriggerEnter(Collider other) 
    {
         if(other.gameObject.CompareTag("SoulObject"))
         {   
            TakeSoul(other.GetComponent<SoulObject>()); 
         }
    }

    void TakeSoul(SoulObject enemySoul)
    {
        enemySoul.transform.parent = null; 
        enemySoul.GetComponent<MeshRenderer>().enabled = true;  
        enemySoul.suckPoint = this.transform.position + new Vector3(0,2,0);
        enemySoul.suckSpeed = soulSuckSpeed; 
        enemySoul.attachedSoulPilar = thisScript; 
        enemySoul.isBeingSucked = true; 
    }

    private void OnDrawGizmosSelected()
    {
    
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, soulCheckRadius);
        
    }

    public void AddSoul(float soulValue)
    {
        currentSouls += soulValue; 
        if(currentSouls >= soulsNeeded && !pilarIsFull)
        {
            pilarIsFull = true; 
            DoFullPilarEffect(); 
        }
    }

    public void DoFullPilarEffect()
    {
         pilarManager.SetText("THE GODS REWARD YOU");  
   
        if(isHealthPilar) playerController.playerState.AddHealth(healthAddAmount); 
        GameObject pilarEffect1 = Instantiate(fullPilarEffect, transform.position, transform.rotation);
        pilarEffect1.AddComponent<CleanUpScript>(); 
        RemovePilar();   

             
    }

    
}
