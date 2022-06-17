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

    //public float limbCheckRadius;  
    public Vector3 limbCheckRadius;

    void Start()
    {
        bloodDripEffect.GetComponent<ParticleSystem>().Pause();

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
        
            if (!hitLimbs.Contains(other.attachedRigidbody) && other.gameObject.CompareTag("Limb"))
            {
                hitLimbs.Add(other.attachedRigidbody);
            }

            if (!hitInsides.Contains(other.gameObject) && other.gameObject.CompareTag("EnemyInside"))
            {
                hitInsides.Add(other.gameObject);
            }
        
        }

    /*

    public void LimbCheck()
    {
        // Collider[] colls = Physics.OverlapSphere(playerController.transform.position, playerController.blockStunRadius);     

        Collider[] limbColls = Physics.OverlapBox(transform.position, limbCheckRadius, transform.rotation); 

        foreach (Collider limb in limbColls)
        {
            if (!hitLimbs.Contains(limb.attachedRigidbody) && limb.gameObject.CompareTag("Limb"))
            {
                hitLimbs.Add(limb.attachedRigidbody);
                print("addlimb"); 
            }

            if (!hitInsides.Contains(limb.gameObject) && limb.gameObject.CompareTag("EnemyInside"))
            {
                hitInsides.Add(limb.gameObject);
            }
        }
    }
    */



    public void CheckForBloodDrip()
    {
          
        if(!canBloodDrip)
        {
            canBloodDrip = true;
            bloodDrip = true;
            //  bloodDripEffect.SetActive(true); 
            bloodDripEffect.GetComponent<ParticleSystem>().Play();
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
            //bloodDripEffect.SetActive(false);  
            canBloodDrip = false;
            bloodDripEffect.GetComponent<ParticleSystem>().Pause();
    }
  
}
    


