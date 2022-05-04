using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class TortiTest : MonoBehaviour
{

    public NavMeshAgent agent; 
    public Transform target ;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        target = GameObject.Find("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
          agent.destination = target.position - transform.forward;
          transform.LookAt(target); 
    }
}
