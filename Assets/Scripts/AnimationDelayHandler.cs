using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelayHandler : MonoBehaviour
{
	// Attach script to animator object

	Animator animToPlay;

	[SerializeField]
	[Tooltip("Delay in seconds prior to animation playback (0 for no delay).")]
	float delay = 0f;

    [SerializeField]
    PathAnimator path;

	void Start()
	{
		animToPlay = gameObject.GetComponent<Animator> ();

		if (delay >= 0f)
		{
			animToPlay.enabled = false;
			StartCoroutine("DelayedPlayback");

            if (path != null)
            {
                path.enabled = false;
            }
        }
		else
		{
			animToPlay.enabled = true;

            if (path != null)
            {
                path.enabled = true;
            }
		}
	}

	IEnumerator DelayedPlayback()
	{
		yield return new WaitForSeconds(delay);
		animToPlay.enabled = true;

        if (path != null)
        {
            path.enabled = true;
        }
	}
}