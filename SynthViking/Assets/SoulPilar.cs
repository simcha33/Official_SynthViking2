using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;

public class SoulPilar : MonoBehaviour
{

    [Header("PILARS STATS")]

    public string pilarType;
    public float soulCheckRadius; 
    public float soulsNeeded;
    public float currentSouls; 
    public float soulSuckSpeed; 
    private float stayTimer;
    public float stayDuration; 
    public bool hasSpawned; 
    public bool deSpawn; 
    private bool pilarIsFull; 


    [Header("Visuals")]
    public GameObject fullPilarEffect; 
    private Material choosenCrystalMat;
    private Material choosenRingMat; 
    

    [Header("Components")]
    public MeshRenderer crystelMeshR; 
    public MeshRenderer PilarRingMeshR;
    public SphereCollider checkCol;
    public GameObject soulRadiusObject; 
    private SoulPilar thisScript;
    private ThirdPerson_PlayerControler playerController;
    private PlayerState playerState; 
    public Animator pilarAnim;
    [HideInInspector]public RewardPilarManager pilarManager; 
    public SpotScript attachedSpot; 
    public TextMeshPro pilarSoulsLeftText; 
  


    [Header("Health Pilar")]
    public bool isHealthPilar;
    public float healthAddAmount; 
    public Material healthPilarRingMat; 
    public Material healthPilarMat;


    [Header("Trap pilar")]
    public bool isTrapPilar; 
    public List<GameObject> trapsToActivate = new List<GameObject>(); 
    public Material trappilarMat; 
    public Material trapPilarRingMat;

    void Start()
    {
       // ResetPilar
        currentSouls = 0; 
        stayTimer = 0;
    
        isHealthPilar = false;
        isTrapPilar = false; 
        pilarManager = GameObject.Find("PilarManager").GetComponent<RewardPilarManager>(); 
        thisScript = gameObject.GetComponent<SoulPilar>(); 
        playerController = GameObject.Find("Player").GetComponent<ThirdPerson_PlayerControler>(); 
        checkCol = gameObject.GetComponent<SphereCollider>(); 
        playerState = playerController.playerState; 
        soulCheckRadius = checkCol.radius;
      //  gameObject.SetActive(false); 
        attachedSpot = GetComponentInParent<SpotScript>(); 
    }

    void Update()
    {
        if(hasSpawned)
        { 
            StayCountdown();
            pilarSoulsLeftText.text = (soulsNeeded - currentSouls).ToString(); 
        }
        if(deSpawn) RemovePilar(); 
        
    }

    void StayCountdown()
    {
        if(stayTimer == 0)
        { 
            pilarAnim.SetTrigger("Spawn");

            if(pilarType == pilarTypes.HealthPilar.ToString())
            {
                isHealthPilar = true;
                isTrapPilar = false; 
                choosenRingMat = healthPilarRingMat; ; 
                choosenCrystalMat = healthPilarMat; 

            }
            else if(pilarType == pilarTypes.TrapPilar.ToString())
            {
                isTrapPilar = true; 
                isHealthPilar = false;
                choosenRingMat = trapPilarRingMat; 
                choosenCrystalMat = trappilarMat; 
            }

            crystelMeshR.material = choosenCrystalMat;
            PilarRingMeshR.material = choosenRingMat; 
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
            attachedSpot.inUse = false; 
            attachedSpot.spotManager.freeSpotList.Add(attachedSpot); 
        }
     
  
    }

    void ResetPilar()
    {
        if(!pilarIsFull) pilarManager.SetText("SHRINE HAS LEFT", null); 
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
        if(!pilarIsFull)
        {
            currentSouls += soulValue; 
            if(currentSouls >= soulsNeeded)
            {
                pilarIsFull = true; 
                DoFullPilarEffect(); 
            }
        }
    }

    public void DoFullPilarEffect()
    {   
        if(isHealthPilar)
        { 
            if(playerState.currentHealth + healthAddAmount > playerState.maxHealth)
            {
                playerState.maxHealth += playerState.currentHealth + healthAddAmount - playerState.maxHealth; 
                pilarManager.SetText("THE GODS REWARD YOU", "+ " + healthAddAmount.ToString() + " health" + "\n" + "MAX HEALTH INCREASED TO " + playerState.maxHealth); 
            }
            else
            {
                pilarManager.SetText("THE GODS REWARD YOU", "+ " + healthAddAmount.ToString() + " health added"); 
            }

            playerState.AddHealth(healthAddAmount);        
        }

        else if(isTrapPilar)
        {
            pilarManager.SetText("THE GODS REWARD YOU", "Heyoo TRAPOIIIOOO"); 
        }

        GameObject pilarEffect1 = Instantiate(fullPilarEffect, transform.position, transform.rotation);
        pilarEffect1.AddComponent<CleanUpScript>(); 
        RemovePilar();              
    }

    
}
