using UnityEngine;
using System.Collections;

public class FishManager {

	private static FishManager mInstance;
	private FishManager()
	{
		
	}
	public static FishManager getInstance()
	{
		if(mInstance == null)
			mInstance = new FishManager();
		return mInstance;
	}

	public void createFish(int fishid)
	{

	}

	public void updateFrame(float dt)
	{

	}
}
