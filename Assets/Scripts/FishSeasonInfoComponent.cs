using UnityEngine;
using System.Collections;
using System.Xml;

public class FishSeasonInfoComponent : MonoBehaviour {
	//public Vector3 center;
	public float speed;
	public float angle;
	public int aiId;
	public Vector2 centerPoint;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public XmlElement getElement(XmlDocument xmlDoc,XmlNode oneseason)
	{
		//创建根节点   
		XmlElement root = xmlDoc.CreateElement("FishSeasonInfo");  
		oneseason.AppendChild(root);
		root.SetAttribute("speed",speed.ToString());
		root.SetAttribute("angle",angle.ToString());
		root.SetAttribute("AIID",aiId.ToString());

		root.SetAttribute("centerPt",centerPoint.x + "," + (centerPoint.y));
		TableFish fishTable = (TableFish)GameTableManager.getInstance().GetTable("table_fish");
		int childcnt = transform.childCount;

        float maxWidth = 0;
        int maxWidthId = 0;
        float minDistanceToView = float.MaxValue;
        float nearestFishWidth = 0;
        for (int i = 0; i < childcnt; i++)
        {
            Transform childTransform = transform.GetChild(i);
            string name = childTransform.gameObject.name;
            if (name.Contains("Prefab_Fish_") == false) continue;
            name = name.Remove(0, 12);
            int fishkindid = fishTable.getFishKindIdByName(name);
            if (fishkindid == -1) continue;
            TableFish.FishRecord record = fishTable.getRecordByFishKindId(fishkindid);
            if (record.width > maxWidth)
            {
                maxWidthId = record.fishId;
                maxWidth = record.width;
            }
            if (minDistanceToView > Mathf.Abs(centerPoint.x - childTransform.localPosition.x))
            {
                minDistanceToView = childTransform.localPosition.x;
                nearestFishWidth = record.width;
            }
        }

		float radian = Mathf.Deg2Rad * (-angle);
		for(int i = 0; i < childcnt; i ++)
		{
			Transform childTransform = transform.GetChild(i);
			Vector3 pos = childTransform.localPosition;
			string name = childTransform.gameObject.name;
			if(name.Contains("Prefab_Fish_") == false) continue;
			name = name.Remove(0,12);
			XmlElement singlefish = xmlDoc.CreateElement("SingleFishOfSeason");
            
			int fishkindid = fishTable.getFishKindIdByName(name);
			if(fishkindid == -1) continue;
			singlefish.SetAttribute("FishKindID",fishkindid.ToString());
            TableFish.FishRecord record = fishTable.getRecordByFishKindId(fishkindid);

			float x0= (pos.x )*Mathf.Cos(radian) - (pos.y)*Mathf.Sin(radian) ;
			float y0= (pos.x )*Mathf.Sin(radian) + (pos.y)*Mathf.Cos(radian) ;

			float dh = 0;
			//childTransform.localPosition = new Vector3(x0,y0,0);
            float temp = maxWidth - record.width - (maxWidth - nearestFishWidth);
            if (fishkindid == maxWidthId) temp = -(maxWidth - nearestFishWidth);
			if(centerPoint.x <= 0)
			{
                //singlefish.SetAttribute("FishPos", (-(x0 / Mathf.Cos(radian))).ToString() + "," + (-(y0 + Mathf.Tan(radian) * Mathf.Abs(x0) - record.width * Mathf.Tan(radian))).ToString());
				singlefish.SetAttribute("FishPos", (-pos.x + temp).ToString() + "," + (-pos.y).ToString());
			}
			else if(centerPoint.x >= 1280)
			{
                singlefish.SetAttribute("FishPos", (pos.x + temp).ToString() + "," + (-pos.y).ToString());
			}
			else if(centerPoint.y <= 0)
			{
                singlefish.SetAttribute("FishPos", (pos.x).ToString() + "," + (pos.y + temp).ToString());
			}
			else if(centerPoint.y >= 720)
			{
                singlefish.SetAttribute("FishPos", (pos.x).ToString() + "," + (-pos.y + temp).ToString());
			}


			root.AppendChild(singlefish);
		}

		return root;
	}
}
