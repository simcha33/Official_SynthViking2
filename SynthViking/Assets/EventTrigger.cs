using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public int eventToTrigger;
    private bool hasBeenTriggered = false;
    public eventManagerScript eventManager;

    private void Start()
    {
       // eventManager = GameObject.Find("EventManager").GetComponent<eventManagerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenTriggered)
        {
            eventManager.EndEvent(); 
            eventManager.SetNewEvent(eventToTrigger);
            hasBeenTriggered = true; 
        }
    }
}
