using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	float deltaTime;
	// Use this for initialization
	void Start ()
	{
		AiPathManager.getInstance().initialize();
		GameTableManager.getInstance().initialize();
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime;
		TimeManager.getInstance().updateFrame(deltaTime);
		FishManager.getInstance().updateFrame(deltaTime);
	}
}
