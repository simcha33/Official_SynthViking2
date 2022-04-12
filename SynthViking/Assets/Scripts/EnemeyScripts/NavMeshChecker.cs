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
 
    private NavMeshAgent agent;
    private float oldLinkCost;

    public NavMeshLink link; 

    public bool checkForEnd; 

    BasicEnemyScript userScript; 
 

   void Awake()
   {
       link = GetComponent<NavMeshLink>(); 
       this.gameObject.transform.localScale = new Vector3(link.width, gameObject.transform.localScale.y, gameObject.transform.localScale.z); 
   }

}

