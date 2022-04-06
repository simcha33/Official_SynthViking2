using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForLimbs : MonoBehaviour
{
    public AttackTargetScript attackScript; 
    public ThirdPerson_PlayerControler playerController; 

    public List <Rigidbody> hitLimbs = new List<Rigidbody>();
    public List<GameObject> hitInsides = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {

        if(!playerController.isAttacking){
           // hitLimbs.Clear(); 
        }
        

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




}
