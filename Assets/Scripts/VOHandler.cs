using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOHandler : MonoBehaviour
{
	AudioSource player;

	void Awake()
	{		
		player = gameObject.GetComponent<AudioSource>();
	}

	void PlayTrack(AudioClip track)
	{
		player.clip = track;
		player.Play();
	}
}
