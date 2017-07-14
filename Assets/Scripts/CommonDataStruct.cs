using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleFishOfSeason
{
	public int mFishKindId;
	public Vector2 mFishPos;
}

public class FishSeasonInfo
{
	public float mSpeed;
	public float mAngle;
	public int mAiId;
	public Vector2 mCenterPoint;
	public List<SingleFishOfSeason> fishList;

	public FishSeasonInfo()
	{
		fishList = new List<SingleFishOfSeason>();
	}

	public void addSingleFish(SingleFishOfSeason singlefish)
	{
		if(fishList != null && singlefish != null)
		{
			fishList.Add(singlefish);
		}
	}

	public SingleFishOfSeason getSingleFish(int index)
	{
		if(fishList == null) return null;
		if(index > -1 && index < fishList.Count)
			return fishList[index];
		return null;
	}
}

public class OneFishSeason
{
	public List<FishSeasonInfo> seasonInfoList;

	public OneFishSeason()
	{
		seasonInfoList = new List<FishSeasonInfo>();
	}

	public void addOneSeasonInfo(FishSeasonInfo seasoninfo)
	{
		if(seasonInfoList != null && seasoninfo != null)
		{
			seasonInfoList.Add(seasoninfo);
		}
	}

	public FishSeasonInfo getSeasonInfo(int index)
	{
		if(seasonInfoList == null) return null;
		if(index > -1 && index < seasonInfoList.Count)
			return seasonInfoList[index];
		return null;
	}

	public int getSeasonInfoCnt()
	{
		return seasonInfoList.Count;
	}
}

public class FishSeasonConfig
{
	public List<OneFishSeason> fishSeasonList;
	
	public FishSeasonConfig()
	{
		fishSeasonList = new List<OneFishSeason>();
	}
	
	public void addOneSeason(OneFishSeason oneseason)
	{
		if(fishSeasonList != null && oneseason != null)
		{
			fishSeasonList.Add(oneseason);
		}
	}
	
	public OneFishSeason getOneSeason(int index)
	{
		if(fishSeasonList == null) return null;
		if(index > -1 && index < fishSeasonList.Count)
			return fishSeasonList[index];
		return null;
	}
	
	public int getSeasonCnt()
	{
		return fishSeasonList.Count;
	}
}






























