using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    public bool isSoundEffect;
    public bool isMusic;

    private AudioSource source;
    private float initialVolume;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        initialVolume = source.volume;
    }

    private void Update()
    {
        if(source.volume != initialVolume * PlayerPrefs.GetFloat("volume"))
        {
            source.volume = initialVolume * PlayerPrefs.GetFloat("volume");
        }

        if ((isMusic && PlayerPrefs.GetInt("musicToggle") == 0) || (isSoundEffect && PlayerPrefs.GetInt("soundEffectToggle") == 0))
        {
            source.mute = true;
        }
        else
        {
            source.mute = false;
        }
    }
}
