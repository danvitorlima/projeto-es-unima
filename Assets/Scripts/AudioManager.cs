using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    // Start is called before the first frame update
    void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.volume *= PlayerPrefs.GetFloat("Volume");
        }
    }
}
