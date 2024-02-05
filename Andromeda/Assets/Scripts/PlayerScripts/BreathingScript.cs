using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingScript : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(Breathe());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Breathe()
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.clip = clip;
        source.Play();
        yield return new WaitForSeconds(clip.length);
        source.Stop();
        yield return new WaitForSeconds(3);
        StartCoroutine(Breathe());

    }
}
