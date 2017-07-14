using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class FishConfigManager
{
	private static FishConfigManager mInstance;
	private FishSeasonConfig mSeasonConfig;

	private FishConfigManager()
	{
		mSeasonConfig = new FishSeasonConfig();
	}

	public static FishConfigManager getInstance()
	{
		if(mInstance == null)
			mInstance = new FishConfigManager();
		return mInstance;
	}

	public void loadFishSeasonConfig(string configPath)
	{
        string filePath = configPath;
        if (false == File.Exists(filePath))
        {
            return;
        }
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(filePath);
		
		XmlNode oneseasonNode = xmlDoc.SelectSingleNode("CapFish/Server/FishSeasonConfig/OneSeason");
		while(oneseasonNode != null)
		{
			if (oneseasonNode.GetType() == typeof(XmlElement))
			{
				XmlElement oneseasonXe = (XmlElement)oneseasonNode;
				if(oneseasonXe.Name == "OneSeason")
				{
					OneFishSeason oneseason = new OneFishSeason();
					mSeasonConfig.addOneSeason(oneseason);
					XmlNode seasonInfoNode = oneseasonNode.FirstChild;
					while(seasonInfoNode != null)
					{
						if(seasonInfoNode.GetType() == typeof(XmlElement))
						{
							XmlElement seasoninfoXe = (XmlElement)seasonInfoNode;
							if(seasoninfoXe.Name == "FishSeasonInfo")
							{
								FishSeasonInfo seasonInfo = new FishSeasonInfo();
								oneseason.addOneSeasonInfo(seasonInfo);
								seasonInfo.mSpeed = float.Parse(seasoninfoXe.GetAttribute("speed"));
								seasonInfo.mAngle = float.Parse(seasoninfoXe.GetAttribute("angle"));
								seasonInfo.mAiId = int.Parse(seasoninfoXe.GetAttribute("AIID"));
								string centerPt = seasoninfoXe.GetAttribute("centerPt");
								string[] xy = centerPt.Split(',');
								float x = float.Parse(xy[0].Trim());
								float y = float.Parse(xy[1].Trim());
								seasonInfo.mCenterPoint = new Vector2(x,y);

								XmlNode singlefishNode = seasonInfoNode.FirstChild;
								while(singlefishNode != null)
								{
									if(singlefishNode.GetType() == typeof(XmlElement))
									{
										XmlElement singleFishXe = (XmlElement)singlefishNode;
										if(singleFishXe.Name == "SingleFishOfSeason")
										{
											SingleFishOfSeason singlefish = new SingleFishOfSeason();
											seasonInfo.addSingleFish(singlefish);
											singlefish.mFishKindId = int.Parse(singleFishXe.GetAttribute("FishKindID"));
											string fishpos = singleFishXe.GetAttribute("FishPos");
											xy = fishpos.Split(',');
											//Debug.Log(fishpos);
											x = float.Parse(xy[0].Trim());
											y = float.Parse(xy[1].Trim());
											//Debug.Log(fishpos);
											singlefish.mFishPos = new Vector2(x,y);
										}
									}
									singlefishNode = singlefishNode.NextSibling;
								}
							}
						}
						seasonInfoNode = seasonInfoNode.NextSibling;
					}
				}
			}
			oneseasonNode = oneseasonNode.NextSibling;
		}
	}

	public OneFishSeason getOneSeason(int index)
	{
		return mSeasonConfig.getOneSeason(index);
	}

	public int getSeasonCnt()
	{
		return mSeasonConfig.fishSeasonList.Count;
	}
}
