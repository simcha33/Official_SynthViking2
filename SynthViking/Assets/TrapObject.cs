using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks; 

public class TrapObject : MonoBehaviour
{
    public bool hasBeenActivated;
    public bool isActive;
    public float activeDuration;
    public float activeTimer;
    [HideInInspector] public SpotScript attachedSpot;
     public RewardPilarManager pilarManager;

    public MMFeedbacks killFeedback;
    public TextMeshPro timeLeftText;
    public AudioSource source;
    public AudioClip bladeTrapSound;

    private void Start()
    {
        attachedSpot = gameObject.GetComponentInParent<SpotScript>();
        pilarManager = GameObject.Find("PilarManager").GetComponent<RewardPilarManager>();
        
    }

    private void Update()
    {
        if (hasBeenActivated) DoTrap();
        if (!hasBeenActivated && isActive) DeactivedTrap(); 
        
    }

    void DeactivedTrap()
    {       
        //Deactivated and remove trap 
        gameObject.SetActive(false);
        activeTimer = 0;
        isActive = false;

        //Free the spot up
        attachedSpot.inUse = false;
        attachedSpot.spotManager.freeSpotList.Add(attachedSpot);
        pilarManager.currentPilarsInScene--;
       // source.loop = false;
     //   attachedSpotSource.Stop(); 
    }

    void DoTrap()
    {
        if (!isActive)
        {
        //    attachedSpotSource.PlayOneShot(bladeTrapSound);
          //  attachedSpotSource.loop = true;
        }

        isActive = true;
        timeLeftText.text = (activeDuration - activeTimer).ToString("F0"); 
        if(pilarManager.spawnManager.waveHasStarted) activeTimer += Time.deltaTime;
        if(activeTimer >= activeDuration)
        {
            hasBeenActivated = false; 
        }
    }
}
