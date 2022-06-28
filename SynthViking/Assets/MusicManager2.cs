using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager2 : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentSong;
    public List<AudioClip> playedSongs = new List<AudioClip>();
    public List<AudioClip> unplayedSongs = new List<AudioClip>();

    public AudioClip introSong;
    public AudioClip choosenSong; 
    public AudioSource source;


    //public float currentSongDuration;
    public float currentSongTimer; 

    public bool songIsDone;
    public bool songIsPlaying; 

   

    

    void Start()
    {
       // ChooseNewSong(rand); 
    }

    // Update is called once per frame
    void Update()
    {
        if(songIsPlaying) PlaySong(); 
    }

    void PlaySong()
    {
        currentSongTimer -= Time.deltaTime;
        if(currentSongTimer <= 0)
        {
            songIsPlaying = false;
            ChooseNewSong(true); 
        }
    }

    public void ChooseNewSong(bool random)
    {
        if (unplayedSongs.Count > 0)
        {
            if (random)
            {
                int randomSong = Random.Range(0, unplayedSongs.Count);
                choosenSong = unplayedSongs[randomSong];
            }
            else
            {
                choosenSong = unplayedSongs[0]; 
            }


            print("this"); 
            source.PlayOneShot(choosenSong); 
            currentSongTimer = choosenSong.length;
            unplayedSongs.Remove(choosenSong);
            playedSongs.Add(choosenSong); 
            songIsPlaying = true;
        }
        else
        {
            ResetRadio(); 
        }
    }

    void ResetRadio()
    {
        foreach(AudioClip song in playedSongs)
        {
            unplayedSongs.Add(song); 
        }

        playedSongs .Clear();
        ChooseNewSong(true); 
    }
}
