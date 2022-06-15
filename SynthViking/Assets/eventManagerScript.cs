using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks; 

public class eventManagerScript : MonoBehaviour
{
    public int totalEvents;
    public int currentEvent;

    public List<GameObject> eventArea = new List<GameObject>();
    public List<MMFeedbacks> eventFeedbacks = new List<MMFeedbacks>();


    [Header("Components")]
    public Camera mainCamera;
    public Light directionalLight; 

    [Header("Event 1: The drop")]
    public float introDuration;
    public MMFeedbacks event1Feedback; 

    void Start()
    {
        currentEvent = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setNewEvent(int eventInt)
    {

        if(eventInt == 1)
        {
            event1Feedback?.PlayFeedbacks(); 
        }
    }
}
