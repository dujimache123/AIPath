using UnityEngine;
using System.Collections;

public class TimeManager
{
	private static TimeManager mInstance;
	private TimeManager()
	{

	}
	public static TimeManager getInstance()
	{
		if(mInstance == null)
			mInstance = new TimeManager();
		return mInstance;
	}

	public void updateFrame(float dt)
	{
		
	}
}
