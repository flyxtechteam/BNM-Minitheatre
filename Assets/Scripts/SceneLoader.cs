
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
	
	[SerializeField]
	private int scene;

	void Start()
	{
		Application.LoadLevel (scene);
	}
}