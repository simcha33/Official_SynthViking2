using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPoolScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody parentRb;
    BloodPoolScript thisScript;
    private float waitTime;
    private float waitDuration = 2f; 

   
    void Start()
    {
        parentRb = transform.parent.GetComponent<Rigidbody>();
        thisScript = GetComponent<BloodPoolScript>(); 
    }

    // Update is called once per frame
    void Update()
    {
        /*
        waitTime += Time.deltaTime;

        if (waitTime >= waitDuration && parentRb.velocity.y < .2f && parentRb.velocity.y > -.2f)
        {
            print("unparent"); 
            transform.parent = null;
            Destroy(thisScript);
     
        }
        */
    }
}
