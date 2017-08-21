using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOHandler : MonoBehaviour
{
    // Array containing both English and Malay VO clips
    // Array size should be even and contain English and Malay VO clips for each set of dialogue
    [Tooltip("Array of VO clips - size should be even and contain English and Malay clips for each set of dialogue.")]
    [SerializeField]
    AudioClip[] VOAudioClipSets;

    [SerializeField]
    UnityEngine.Audio.AudioMixerGroup mixerGroup;

    AudioSource source;

    bool init = false;

    // Counter used to cycle through VO sets each time audio is played
    int currentSet = 0;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixerGroup;
    }

    void Start()
    {
        init = true;
    }

    void OnEnable()
    {
        if (init)
        {
            Debug.Log("Current: " + currentSet.ToString() + ", Language: " + GlobalData.language.ToString() + ", VO: " + ((currentSet * 2) + GlobalData.language).ToString());
            source.clip = VOAudioClipSets[(currentSet * 2) + GlobalData.language];
            source.Play();
            currentSet++;
        }
    }
}