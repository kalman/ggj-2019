using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab2AnimationScript : MonoBehaviour
{
    public bool playLow = false;
    public bool playHigh = false;
    public AudioSource lowAudio;
    public AudioSource highAudio;

    private bool prevPlayLow = false;
    private bool prevPlayHigh = false;

    void Update()
    {
        if (playLow && !prevPlayLow)
        {
            lowAudio.Play();
        }
        if (playHigh && !prevPlayHigh)
        {
            highAudio.Play();
        }
        prevPlayLow = playLow;
        prevPlayHigh = playHigh;
    }
}
