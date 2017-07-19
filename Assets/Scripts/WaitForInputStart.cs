using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForInputStart : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Time.timeScale = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space))
		{
			Time.timeScale = 1f;
		}
	}
}
