using UnityEngine;
using UnityEditor;
using System.Collections;

public class EventManager
{

	private static EventManager mInstance;
	private EventManager()
	{
		
	}
	public static EventManager getInstance()
	{
		if(mInstance == null)
			mInstance = new EventManager();
		return mInstance;
	}

	public void onEventFishSeason(int seasonIndex, bool modifyFlag = false)
	{
        if (modifyFlag)
        {
            this.onModifyOneSeason(seasonIndex);
            return;
        }
		GameObject sourcePoint = GameObject.Find("Anchor_BottomLeft");
        Transform objBottomLeft = sourcePoint.transform;
        int childCnt = objBottomLeft.childCount;
        for (int i = 0; i < childCnt; )
        {
            Transform child = objBottomLeft.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject);
            childCnt = objBottomLeft.childCount;
        }

		OneFishSeason season = FishConfigManager.getInstance().getOneSeason(seasonIndex);
		TableFish fishtable = (TableFish)GameTableManager.getInstance().GetTable("table_fish");
		foreach(FishSeasonInfo seasoninfo in season.seasonInfoList)
		{
			foreach(SingleFishOfSeason singlefish in seasoninfo.fishList)
			{
				TableFish.FishRecord record = fishtable.getRecordByFishKindId(singlefish.mFishKindId);
				if(record == null) continue;
				float fFishLength = record.width;
				//Debug.Log(record.name);
				GameObject fishObj = (GameObject)GameObject.Instantiate(Resources.Load("FishPrefabs/Prefab_Fish_" + record.name));
				fishObj.transform.parent = sourcePoint.transform;
				fishObj.transform.localScale = Vector3.one * record.scaleFactor;
				//fishObj.transform.localPosition = new Vector3(singlefish.mFishPos.x + seasoninfo.mCenterPoint.x-1200,singlefish.mFishPos.y + seasoninfo.mCenterPoint.y,0);
				Fish fishComponent = fishObj.AddComponent<Fish>();

				fishComponent.Rotation = seasoninfo.mAngle;
				fishComponent.AiPath = AiPathManager.getInstance().getPath(seasoninfo.mAiId);
                fishComponent.BaseSpeed = seasoninfo.mSpeed;
				fishComponent.FishWidth = record.width;

				float fDelay = 0.0f;
				if (seasoninfo.mCenterPoint.x <= 0)
				{
					fishObj.transform.localPosition = new Vector3(-fFishLength,seasoninfo.mCenterPoint.y + singlefish.mFishPos.y,0);
					//fishObj.transform.localPosition.y = seasoninfo.mCenterPoint.y + singlefish.mFishPos.y;
					fDelay = (singlefish.mFishPos.x - seasoninfo.mCenterPoint.x) / seasoninfo.mSpeed;
				}
				else if (seasoninfo.mCenterPoint.x >= 1280)
				{
					fishObj.transform.localPosition = new Vector3(1280 + fFishLength,seasoninfo.mCenterPoint.y + singlefish.mFishPos.y,0);
					//fishObj.transform.localPosition.y = seasoninfo.mCenterPoint.y + singlefish.mFishPos.y;
					fDelay = (singlefish.mFishPos.x + seasoninfo.mCenterPoint.x - 1280) /seasoninfo.mSpeed;
				}
				else if (seasoninfo.mCenterPoint.y <= 0)
				{
					fishObj.transform.localPosition = new Vector3(seasoninfo.mCenterPoint.x + singlefish.mFishPos.x,-fFishLength,0);
					//fishObj.transform.localPosition.y = (-1) * fFishLength;
					fDelay = (singlefish.mFishPos.y - seasoninfo.mCenterPoint.y) /seasoninfo.mSpeed;
				}
				else if (seasoninfo.mCenterPoint.y >= 720)
				{
					fishObj.transform.localPosition = new Vector3 (seasoninfo.mCenterPoint.x + singlefish.mFishPos.x,720 + fFishLength,0);
					//fishObj.transform.localPosition.y = 720 + fFishLength;
					fDelay = (singlefish.mFishPos.y + seasoninfo.mCenterPoint.y - 720) /seasoninfo.mSpeed;
				}
				else
				{
					fishObj.transform.localPosition = new Vector3(seasoninfo.mCenterPoint.x + singlefish.mFishPos.x,seasoninfo.mCenterPoint.y + singlefish.mFishPos.y,0);
					//fishObj.transform.localPosition.y = (seasoninfo.mCenterPoint.y + singlefish.mFishPos.y);
					fDelay = 0.0f;
				}

				Vector3 pos = fishObj.transform.localPosition;
				fishObj.transform.localPosition = new Vector3(pos.x,720-pos.y,0);
                if (seasonIndex == 4)
                {
                    Debug.Log(fishObj.transform.localPosition + "        " + fDelay);
                }
                
				fishComponent.DelayActiveTime = fDelay;
			}
		}
	}

    public void onModifyOneSeason(int seasonIndex)
    {
        GameObject seasonRoot = GameObject.FindWithTag("SeasonRoot");
        Transform objRoot = seasonRoot.transform;
        int childCnt = objRoot.childCount;
        for (int i = 0; i < childCnt; )
        {
            Transform child = objRoot.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject);
            childCnt = objRoot.childCount;
        }

        OneFishSeason season = FishConfigManager.getInstance().getOneSeason(seasonIndex);
        TableFish fishtable = (TableFish)GameTableManager.getInstance().GetTable("table_fish");

        GameObject oneSeasonObj = new GameObject();
        oneSeasonObj.transform.parent = objRoot;
        oneSeasonObj.AddComponent<OneSeasonComponent>();
        oneSeasonObj.name = "OneSeason";
        oneSeasonObj.transform.localScale = Vector3.one;
        oneSeasonObj.transform.localPosition = Vector3.zero;

        foreach (FishSeasonInfo seasoninfo in season.seasonInfoList)
        {
            GameObject seasonInfoObj = new GameObject();
            seasonInfoObj.transform.parent = oneSeasonObj.transform;
            seasonInfoObj.name = "OneSeasonInfo";
            seasonInfoObj.transform.localScale = Vector3.one;
            
            FishSeasonInfoComponent seasonInfoCom = seasonInfoObj.AddComponent<FishSeasonInfoComponent>();
            seasonInfoCom.centerPoint.Set(seasoninfo.mCenterPoint.x, seasoninfo.mCenterPoint.y);
            seasonInfoCom.speed = seasoninfo.mSpeed;
            seasonInfoCom.aiId = seasoninfo.mAiId;
            seasonInfoCom.angle = seasoninfo.mAngle;

            seasonInfoCom.transform.localPosition = new Vector3(seasoninfo.mCenterPoint.x, -(seasoninfo.mCenterPoint.y), 0);

            foreach (SingleFishOfSeason singlefish in seasoninfo.fishList)
            {
                TableFish.FishRecord record = fishtable.getRecordByFishKindId(singlefish.mFishKindId);
                if (record == null) continue;
                float fFishLength = record.width;

                GameObject fishObj = (GameObject)GameObject.Instantiate(Resources.Load("FishPrefabs/Prefab_Fish_" + record.name));
                fishObj.name = "Prefab_Fish_" + record.name;
                fishObj.transform.parent = seasonInfoObj.transform;
                fishObj.transform.localScale = Vector3.one * record.scaleFactor;
                fishObj.transform.localPosition = new Vector3(singlefish.mFishPos.x, singlefish.mFishPos.y, 0);

                if (seasoninfo.mCenterPoint.x <= 0)
                {
                    fishObj.transform.localPosition = new Vector3(-singlefish.mFishPos.x, singlefish.mFishPos.y, 0);
                }
                else if (seasoninfo.mCenterPoint.x >= 1280)
                {
                    fishObj.transform.localPosition = new Vector3(singlefish.mFishPos.x, singlefish.mFishPos.y, 0);
                }
                else if (seasoninfo.mCenterPoint.y <= 0)
                {
                    fishObj.transform.localPosition = new Vector3(singlefish.mFishPos.x, -singlefish.mFishPos.y, 0);
                }
                else if (seasoninfo.mCenterPoint.y >= 720)
                {
                    fishObj.transform.localPosition = new Vector3(singlefish.mFishPos.x, singlefish.mFishPos.y, 0);
                }
            }

            UnityEditor.EditorApplication.MarkSceneDirty();
        }
    }
}
