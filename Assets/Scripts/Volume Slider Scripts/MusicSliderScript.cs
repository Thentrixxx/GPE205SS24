using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSliderScript : MonoBehaviour
{
    public AudioMixer musicAudioMixer;
    public Slider musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        OnMusicVolumeChange();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMusicVolumeChange()
    {
        // Start with the slider value (assuming our slider runs from 0 to 1)
        float newVolume = musicSlider.value;

        if (newVolume <= 0)
        {
            // If we are at zero, set our volume to the lowest value
            newVolume = -80;
        }

        else
        {
            // We are >0, so start by finding the log10 value
            newVolume = Mathf.Log10(newVolume);

            //Make it in the 0-20db range (instead of 0-1 db)
            newVolume = newVolume * 20;
        }

        //Set the volume to the new volume setting
        musicAudioMixer.SetFloat("MusicVolume", newVolume);
    }
}
