using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerHandler : MonoBehaviour
{
	// Attach script to collider object
	// Animator component of object to be animated should be disabled in scene

	[SerializeField]
	[Tooltip("Animator object to be triggered.")]
	Animator animToPlay;

	[SerializeField]
	[Tooltip("Specify a delay prior to animation playback (0 for no delay).")]
	float delay = 0f;

    [SerializeField]
    PathAnimator path;

	void Start()
	{
		animToPlay.enabled = false;

        if (path != null)
        {
            path.enabled = false;
        }
    }

	void OnTriggerEnter()
	{

		if (delay >= 0f)
		{
			StartCoroutine("DelayedPlayback");
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