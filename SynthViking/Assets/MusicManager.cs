using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro; 

public class MusicManager : MonoBehaviour
{

    public int currentSong;
    public TextMeshPro songTitle;
    public float songVolume;
    public MMFeedbacks songPlayer;


    // Start is called before the first frame update
    void Start()
    {
        songPlayer?.PlayFeedbacks();
        MMFeedbackSound sound = songPlayer.GetComponent<MMFeedbackSound>();
        sound.SetDelayBetweenRepeats(sound.FeedbackDuration); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
