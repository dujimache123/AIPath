using UnityEngine;
using System.Collections;
using System.Xml;

public class OneSeasonComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void exportXml(string xmlname)
	{
		XmlDocument xmlDoc = new XmlDocument();
		//创建根节点
		XmlElement capfish = xmlDoc.CreateElement("CapFish");
		xmlDoc.AppendChild(capfish);
		XmlElement server = xmlDoc.CreateElement("Server");  
		capfish.AppendChild(server);
		XmlElement fishseason = xmlDoc.CreateElement("FishSeasonConfig");  
		server.AppendChild(fishseason);
		XmlElement oneseason = xmlDoc.CreateElement("OneSeason");  
		fishseason.AppendChild(oneseason);
		
		int childcnt = transform.childCount;
		for(int i = 0; i < childcnt; i ++)
		{
			FishSeasonInfoComponent seasoninfoCom = transform.GetChild(i).GetComponent<FishSeasonInfoComponent>();
			XmlElement seasoninfo = seasoninfoCom.getElement(xmlDoc,oneseason);
			oneseason.AppendChild(seasoninfo);
		}
		xmlDoc.Save(Application.dataPath + "/" + xmlname);
	}
}
