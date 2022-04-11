/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class NavMeshChecker : MonoBehaviour
{

    public NavMeshAgent agent; 
    public OffMeshLink offLink;
    public NavMeshLink navLink; 
    public Transform currentLink;  
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
 
using UnityEngine;
using System.Collections;
using UnityEngine.AI; 
 
public class NavMeshChecker : MonoBehaviour {
 
   // public Transform goal;
 
    private NavMeshAgent agent;
    //public OffMeshLink link;
    private float oldLinkCost;

    public OffMeshLink[] links; 
 
   
   void start(){
     //  GameObjects.Find()
     links = GameObject.FindObjectsOfType<OffMeshLink>(); 
   }

   void Update()
   {
       foreach(OffMeshLink offLink in links){
           if(offLink.activated)
           {
           //    offLink.
               //offLink.enabled = false; 
           }
           else if(!offLink.isActiveAndEnabled)
       }
   }

}

