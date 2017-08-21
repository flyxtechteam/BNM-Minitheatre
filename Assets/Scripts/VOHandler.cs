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

    [SerializeField]
    [Tooltip("Override - change language to Malay. For testing purposes only. Make sure to uncheck before build.")]
    bool LanguageOverrideToMalay = false;

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
        Debug.Log("Initialized!");
    }

    void OnEnable()
    {
        if (init)
        {
            Debug.Log("Current: " + currentSet.ToString() + ", Language: " + GlobalData.language.ToString() + ", VO: " + ((currentSet * 2) + GlobalData.language).ToString());

            if (!LanguageOverrideToMalay)
            {
                source.clip = VOAudioClipSets[(currentSet * 2) + GlobalData.language];
            }
            else
            {
                source.clip = VOAudioClipSets[(currentSet * 2) + 1];
            }
            
            source.Play();

            if (currentSet < VOAudioClipSets.Length - 1)
            {
                currentSet++;
            }
            else
            {
                currentSet = 0;
            }
        }
    }
}