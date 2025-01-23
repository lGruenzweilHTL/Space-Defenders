using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicClips;

    void Start()
    {
        musicSource.clip = musicClips[Random.Range(0, musicClips.Length)];
        musicSource.Play();
    }

    void Update()
    {
        for (int i = 0; i < musicClips.Length; i++)
        {
            //if not playing a song
            if (musicSource.clip == musicClips[i] && !musicSource.isPlaying)
            {
                //if last song in the queue
                if (musicClips[i] == musicClips[musicClips.Length - 1])
                {
                    //restarting the queue
                    musicSource.clip = musicClips[0];
                    musicSource.Play();
                }
                else
                {
                    //playing the next song in the queue
                    musicSource.clip = musicClips[i + 1];
                    musicSource.Play();
                }
            }
        }
    }
}