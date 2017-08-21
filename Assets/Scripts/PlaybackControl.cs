using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlaybackControl : MonoBehaviour
{
    [SerializeField]
    Animator pauseOverlay;

    [SerializeField]
    PlayableDirector timeline;

    [SerializeField]
    float pauseDelay = 5f;

    bool pause = false;
    float scaleRate = 0.05f;
    float targetTimeScale = 1f;
    float LerpThreshold = 0.05f;
    float audioFadeDuration = 2f;
    
    void Update ()
	{
		if ((!Input.GetKey(GlobalData.key_cycle)) && (!pause))
		{
            StartCoroutine("DelayPause");
            pause = true;
        }
		
		if (Input.GetKeyDown(GlobalData.key_cycle))
		{ 
            StopCoroutine("DelayPause");
            targetTimeScale = 1f;
            pause = false;

            pauseOverlay.SetBool("paused", false);
        }

        if (targetTimeScale == 1f)
        {
            if (Time.timeScale < (1f - LerpThreshold))
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, scaleRate);
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            if (targetTimeScale == 0f)
            {
                if (Time.timeScale > LerpThreshold)
                {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, scaleRate);
                }
                else
                {
                    Time.timeScale = 0f;
                }
            }
        }

        if (timeline.time < audioFadeDuration)
        {
            AudioListener.volume = (float) (timeline.time / audioFadeDuration);
        }
        else if (timeline.time >= timeline.duration - audioFadeDuration)
        {
            AudioListener.volume = (float) (1f - ((timeline.time - (timeline.duration - audioFadeDuration)) / audioFadeDuration));
        }
	}

    IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(pauseDelay);

        if (pause == true)
        {
            targetTimeScale = 0f;
            pauseOverlay.SetBool("paused", true);
            pauseOverlay.gameObject.SetActive(true);
        }
    }
}
