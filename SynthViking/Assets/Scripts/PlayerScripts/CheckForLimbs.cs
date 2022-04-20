using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForLimbs : MonoBehaviour
{
    public AttackTargetScript attackScript; 
    public ThirdPerson_PlayerControler playerController; 

    public List <Rigidbody> hitLimbs = new List<Rigidbody>();
    public List<GameObject> hitInsides = new List<GameObject>();

    [Header ("BLOOD DRIP EFFECT")]
    public bool bloodDrip;

    public bool canBloodDrip; 

    public GameObject bloodDripEffect; 

    public float bloodDripDuration; 

    public float bloodDripTimer; 

    public Transform axeBlade; 

    void Start()
    {

  
    }


    // Update is called once per frame
    void Update()
    {
       // DoBloodDrip(); 
        if(!playerController.isAttacking){
           // hitLimbs.Clear(); 
            
        }

        DoBloodDrip(); 

        
        

    }


    void OnTriggerEnter(Collider other)
    {
        if(!hitLimbs.Contains(other.attachedRigidbody) && other.gameObject.CompareTag("Limb"))
        {
            hitLimbs.Add(other.attachedRigidbody);   
        }   

        if(!hitInsides.Contains(other.gameObject) && other.gameObject.CompareTag("EnemyInside")){
            hitInsides.Add(other.gameObject);
        }
    }


    

    public void CheckForBloodDrip()
    {
          
        if(!canBloodDrip)
        {
            canBloodDrip = true;
            bloodDrip = true;           
            bloodDripEffect.SetActive(true); 
        }
        
        bloodDripTimer = 0f; 
               
    }

    public void DoBloodDrip()
    {
        if(canBloodDrip && !playerController.isBlocking)
        {
            bloodDrip = false;         
         
            //Stop drip
            bloodDripTimer += Time.deltaTime; 
            if(bloodDripTimer >= bloodDripDuration)
            {
                canBloodDrip = false; 
            }
        }
        else
        {
            StopBloodDrip(); 
        }
    }


    public void StopBloodDrip()
    {
            bloodDripEffect.SetActive(false);  
            canBloodDrip = false;     
    }
  
}
    


