using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotScript : MonoBehaviour
{
    public bool inUse;
    public string useType; 
    [HideInInspector] public ObjectSpotManager spotManager;
    [HideInInspector] public SoulPilar rewardPilar;
    public GameObject bladeTrap; 
    public GameObject hamerTrap; 

    public List<GameObject> trapList = new List<GameObject>(); 


    void Start()
    {
        spotManager = GameObject.Find("ObjectSpotManager").GetComponent<ObjectSpotManager>(); 
        rewardPilar = GetComponentInChildren<SoulPilar>();
        inUse = false; 
        rewardPilar.gameObject.SetActive(false); 

        foreach(GameObject trap in trapList)
        {
            trap.gameObject.SetActive(false); 
        }

     
    }

}
