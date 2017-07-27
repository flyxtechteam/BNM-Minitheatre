using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackControl : MonoBehaviour
{
	[SerializeField]
	KeyCode cycleKey = KeyCode.W;

    [SerializeField]
    Animator pauseOverlay;

    [SerializeField]
    float pauseDelay = 5f;

    bool pause = false;
    float scaleRate = 0.05f;
    float targetTimeScale = 0f;
    float LerpThreshold = 0.05f;


    void Update ()
	{
		if ((!Input.GetKey(cycleKey)) && (!pause))
		{
            StartCoroutine("DelayPause");
            pause = true;
        }
		
		if (Input.GetKeyDown(cycleKey))
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
	}

    IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(pauseDelay);

        if (pause == true)
        {
            targetTimeScale = 0f;
            pauseOverlay.SetBool("paused", true);
        }
    }
}
