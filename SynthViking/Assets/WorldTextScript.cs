using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextScript : MonoBehaviour
{
    public Camera cam; 
    void Start()
    {
           cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
       // transform.LookAt(cam.transform.position);
       /*
            float singleStep = 23f * Time.deltaTime;
            Vector3 targetDir = cam.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            */
      
    }
}
