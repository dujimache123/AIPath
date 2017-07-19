using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	float deltaTime;
	// Use this for initialization
	void Start ()
	{
		AiPathManager.getInstance().initialize(Application.dataPath + "/Resources/Configs/table_ai.bytes");
		GameTableManager.getInstance().initialize();
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime;
		TimeManager.getInstance().updateFrame(deltaTime);
		FishManager.getInstance().updateFrame(deltaTime);
	}
}
