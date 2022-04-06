using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEnemyInside : MonoBehaviour
{

    public GameObject enemyInside;
    public GameObject attachedLimb;
   // public GameObject[] insidesToActivate;
    public List<GameObject> insidesToActivate = new List<GameObject>();

    private void Start()
    {
        //enemyInside = GetComponentInChildren
        //enemyInside = 
        // foreach(GameObject obj in )

       
        
        if(gameObject.GetComponentInChildren<InsideRefference>().gameObject != null)
        {
            enemyInside = gameObject.GetComponentInChildren<InsideRefference>().gameObject;
            insidesToActivate.Add(enemyInside);
            enemyInside.GetComponent<MeshRenderer>().enabled = false;
        }
        // enemyInside = gameObject.GetComponent <GameObject.of> ();  

        attachedLimb = this.transform.parent.gameObject; 

       // if(attachedLimb.GetComponentInChildren<InsideRefference>())

        if (this.transform.parent.gameObject.GetComponent<SetEnemyInside>() != null)
        {
            // attachedLimb = this.transform.parent.gameObject;
            //  insidesToActivate.Add(attachedLimb.GetComponent<SetEnemyInside>().enemyInside);

        }

       // insidesToActivate.Add(attachedLimb);


        //if(gameObject.GetComponent<InsideRefference>() != null) enemyInside.GetComponent<MeshRenderer>().enabled = false;




    }

    public void ActivateLimbInside()
    {
        foreach(GameObject inside in insidesToActivate)
        {
            // inside.SetActive(true); 
            inside.GetComponent<MeshRenderer>().enabled = true; 
        }
    }

}
