using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
	[SerializeField]
	Image fadeBlack, fadeWhite;

	[SerializeField]
	[Tooltip("The rate of change in seconds at which the transition plays (0.0 - 1.0).")]
	float fadeRate = 0.3f;

	// fadeTo - colour of fade (white/black)
	// inout - +/- multiplier for fading in/out
	int fadeTo, inout;

	// current alpha value of fade
	float fadeValue;
		
	// set true to update transition
	bool fade;

	void FadeIn(int colour)
	{
		fadeTo = colour;
		fadeValue = 1f;
		inout = -1;
		fade = true;
	}

	void FadeOut(int colour)
	{
		fadeTo = colour;
		fadeValue = 0f;
		inout = 1;
		fade = true;
	}

//
//	// colour = 0 - black, 1 - white
//	// mode = 0 - in, 1 - out
//	void Fade(int colour, int mode)
//	{
//		fadeTo = colour;
//		fade = true;
//
//		if (mode == 0)
//		{
//			fadeValue = 1f;
//			inout = -1;
//		}
//		else
//		{
//			fadeValue = 0f;
//			inout = 1;
//		}
//	}

	void Update()
	{
		if (fade)
		{

			if (fadeTo == 1) {
				fadeWhite.color = new Color (1f, 1f, 1f, fadeValue);
			}

			// if fadeTo any value other than 1 (e.g. 0)
			// also acts as default for values other than 0 or 1
			else
			{
				fadeBlack.color = new Color (0f, 0f, 0f, fadeValue);
			}

			// update fadeValue
			fadeValue += inout * fadeRate * Time.deltaTime;

			// check if fadeValue has exceeded the normal range, stop fade if true
			if ((fadeValue <= 0f) || (fadeValue >= 1f))
			{
				fade = false;
			}
		}
	}
}