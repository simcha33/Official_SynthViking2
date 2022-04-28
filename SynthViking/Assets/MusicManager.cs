using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;


public class MusicManager : MonoBehaviour
{

    //public TextMeshPro songTitle;
    public string currentTextString; 
    public float songVolume;
    public MMFeedbacks currentSong;
    public List<MMFeedbacks> unplayedmusicList = new List<MMFeedbacks>();
    public List<MMFeedbacks> playedmusicList = new List<MMFeedbacks>();
    public bool songIsFinished;
    public bool canChangeSong; 
    
    public float songDuration;
    public float songTimer; 

    // Start is called before the first frame update
    void Start()
    {
        songIsFinished = true;
        canChangeSong = true; 
    }

    void Update()
    {
        CheckForSong(); 
    }

    void CheckForSong()
    {
        songTimer += Time.unscaledDeltaTime;
        if (songTimer >= songDuration) songIsFinished = true; 
        if (songIsFinished && canChangeSong) ChooseNextSong();       
    }

    void ChooseNextSong()
    {
        //Set next song
        int choosenSong = Random.Range(0, unplayedmusicList.Count + 1); 
        currentSong = unplayedmusicList[choosenSong];
        songDuration = currentSong.GetComponent<MMFeedbackSound>().FeedbackDuration;
        currentTextString = currentSong.GetComponent<MMFeedbackSound>().name;
        currentSong?.PlayFeedbacks();
   
        //Change lists
        if(unplayedmusicList.Contains(currentSong)) unplayedmusicList.Remove(currentSong);
        playedmusicList.Add(currentSong);

        //Set default values back
        songIsFinished = false;
        songTimer = 0f; 

        //Check if all the songs have been played
        if(unplayedmusicList.Count <= 0) ResetPlaylist(); 
    }

    void ResetPlaylist()
    {
        foreach(MMFeedbacks usedSong in playedmusicList)
        {
            unplayedmusicList.Add(usedSong);
            playedmusicList.Remove(usedSong); 
        }
    }

    

}