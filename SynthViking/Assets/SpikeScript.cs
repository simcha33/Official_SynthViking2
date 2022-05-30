using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public Transform anchorPoint;
    public int bodyCount;
    public bool canPullTarget;
    public bool isPullingTarget; 
    public Rigidbody targetRb;
    public float pullForce;
    public BasicEnemyScript targetScript;
    public List<Transform> stuckList = new List<Transform>(); 

    void Start()
    {
        canPullTarget = true; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(isPullingTarget) MoveTarget();
        if (bodyCount > 0) HoldTargets(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy") && canPullTarget)
        {
            BasicEnemyScript enemyScript = targetScript = other.transform.GetComponent<BasicEnemyScript>();
            if (enemyScript.isLaunched)
            {
                canPullTarget = false;
                isPullingTarget = true;
                print("pull?"); 
                targetRb = enemyScript.enemyRb;
            }
        }
    }

    void MoveTarget()
    {
        print("moveee??");
        // Vector3 destionation = targetRb.position - anchorPoint.position;
        //  targetRb.AddForce(destionation * pullForce); 

        targetRb.transform.position = Vector3.MoveTowards(targetRb.transform.position, anchorPoint.position, 3f);

        if (Vector3.Distance(targetRb.transform.position, anchorPoint.position) < .2f)
        {

            stuckList.Add(targetRb.transform); 
            targetScript.TakeDamage(targetScript.maxHealth, "EnvironmentDamage");
           
            foreach (Rigidbody rb in targetScript.ragdollRbs)
            {
                rb.velocity = new Vector3(0, 0, 0);
            }

            targetRb.transform.position = anchorPoint.position;
            targetRb.transform.parent = anchorPoint;
            bodyCount++; 
            canPullTarget = true;
            isPullingTarget = false;
            targetScript = null;
            targetRb = null;
        }
    }

    void HoldTargets()
    {
        foreach(Transform obj in stuckList)
        {
            obj.transform.position = anchorPoint.transform.position; 
        }
    }
}
