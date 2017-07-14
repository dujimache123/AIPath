using UnityEngine;
using System.Collections;

public class SeasonControl : MonoBehaviour {

	int seasonIndex = -1;
	public Transform objBottomLeft;
	public UILabel mLabelSeasonIndex;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClickLastSeason()
	{
		seasonIndex --;
		if(seasonIndex >= 0)
		{
			int childCnt = objBottomLeft.childCount;
			for(int i = 0; i < childCnt;)
			{
				Transform child = objBottomLeft.GetChild(i);
				GameObject.DestroyImmediate(child.gameObject);
				childCnt = objBottomLeft.childCount;
			}
			EventManager.getInstance().onEventFishSeason(seasonIndex);
			mLabelSeasonIndex.text = seasonIndex.ToString();
		}
		else
		{
			seasonIndex = 0;
		}
	}

	public void onClickNextSeason()
	{
		seasonIndex ++;
		if(seasonIndex < FishConfigManager.getInstance().getSeasonCnt())
		{
			int childCnt = objBottomLeft.childCount;
			for(int i = 0; i < childCnt;)
			{
				Transform child = objBottomLeft.GetChild(i);
				GameObject.DestroyImmediate(child.gameObject);
				childCnt = objBottomLeft.childCount;
			}
			EventManager.getInstance().onEventFishSeason(seasonIndex);
			mLabelSeasonIndex.text = seasonIndex.ToString();
		}
		else
		{
			seasonIndex = FishConfigManager.getInstance().getSeasonCnt() - 1;
		}
	}
}
