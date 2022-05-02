using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class HomingProjectile : MonoBehaviour
{

    public float aliveDuration;
    public float aliveTimer;
    [HideInInspector] public bool hasTouchedTarget;
    private Rigidbody rb; 
    public float moveSpeed;
    private float targetDistance;
    public bool maxSpeedReached; 
     Vector3 maxvelocity; 
    public Transform target; 

    public float projectileDamage; 

    public 


    void Start()
    {
        SetTarget(); 
        aliveTimer = 0f; 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
      //  ChaseTarget(); 
    }

     void FixedUpdate() 
     {
        ChaseTarget();    
    }

    void SetTarget()
    {
        target = GameObject.Find("Player").transform; 
    }

    void ChaseTarget()
    {
       // transform.position = Vector3.MoveTowards(transform.position, target.position  + new Vector3(0,2f,0), moveSpeed * Time.fixedDeltaTime); 
        rb.AddForce((target.transform.position- transform.position) * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange); 
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed); 

        /*
        if(rb.velocity.magnitude >= moveSpeed)
        {
            if(!maxSpeedReached)
            { 
                maxvelocity = rb.velocity; 
                maxSpeedReached =  true;
            }

            rb.velocity = maxvelocity; 
        }
        else{
            maxSpeedReached = false; 
        }
        
        aliveTimer += Time.deltaTime;
        if(aliveTimer >= aliveDuration) DestroyProjectile(); 

        */


    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerState>().TakeDamage(projectileDamage, "Projectile");
            hasTouchedTarget = true; 
            DestroyProjectile();  
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Envorinment"))
        {
            DestroyProjectile();  
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<BasicEnemyScript>().TakeDamage(projectileDamage, "Projectile"); 
        }
    }

    void Damagetarget()
    {

    }

    void DestroyProjectile()
    {   
        //Play some kind of an effect
        Destroy(gameObject); 
    }

    // Update is called once per frame
   
}
