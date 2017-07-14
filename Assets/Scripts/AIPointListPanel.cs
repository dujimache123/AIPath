using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPointListPanel : MonoBehaviour {
	public GameObject mPointItem;
	public UIGrid mGrid;
	public Transform highlight;
	GameObject prePointItem;
	public UIScrollView mScrollView;
	public UIPanel mPanel;
	public PathControl mPathControl;
	// Use this for initialization
	void Start () {
		prePointItem = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addPoint(AiPoint point)
	{
		GameObject item = (GameObject)GameObject.Instantiate(mPointItem);
		item.SetActive(true);
		item.name = "point" + point.id.ToString();
		item.transform.parent = mGrid.transform;
		mGrid.AddChild(item.transform);
		item.transform.localScale = new Vector3(1,1,1);
		item.transform.FindChild("UILabel_AIId").GetComponent<UILabel>().text = point.id.ToString();
		item.transform.FindChild("UILabel_SpeedScale").GetComponent<UILabel>().text = point.speedScale.ToString();
		item.transform.FindChild("UILabel_Rchange").GetComponent<UILabel>().text = point.rChange.ToString();
		item.transform.FindChild("UILabel_During").GetComponent<UILabel>().text = point.time.ToString();
		item.GetComponent<UIButtonMessage>().functionName = "onClickPointItem";


		GameObject highlight = item.transform.FindChild("HighLight").gameObject;
		highlight.SetActive(true);
		highlight.GetComponent<UISprite>().enabled = true;
		if(prePointItem)
		{
			prePointItem.transform.FindChild("HighLight").gameObject.SetActive(false);
		}
		mGrid.repositionNow = true;
		prePointItem = item;

	}

	public void onClickPointItem(GameObject obj)
	{
		if(prePointItem == null)
		{
			obj.transform.FindChild("HighLight").gameObject.SetActive(true);
			prePointItem = obj;
		}
		if(prePointItem.name != obj.name)
		{
			prePointItem.transform.FindChild("HighLight").gameObject.SetActive(false);
			obj.transform.FindChild("HighLight").gameObject.SetActive(true);
			prePointItem = obj;
		}
	}

	public int getSelectedItemIndex()
	{
		List<Transform> itemlist = mGrid.GetChildList();
		int index = 0;
		foreach(Transform item in itemlist)
		{
			GameObject highlightobj = item.transform.FindChild("HighLight").gameObject;
			if(highlightobj.activeSelf == true)
				return index;
			index ++;
		}
		return -1;
	}

	public void removeItem(int index)
	{
		if(index < 0 || index > mGrid.GetChildList().Count)
			return;
        //Transform item = mGrid.RemoveChild(index);
        //GameObject.DestroyImmediate(item.gameObject);
        //mPanel.Invalidate(true);
        //this.updateAllItem();
	}

	public void updateAllItem()
	{
		int index = 0;
		List<Transform> itemlist = mGrid.GetChildList();
		foreach(Transform item in itemlist)
		{
			UILabel idlabel = item.transform.FindChild("UILabel_AIId").GetComponent<UILabel>();
			idlabel.text = index.ToString();
			index ++;
		}
	}

	public void updateItem(int index,AiPoint point)
	{
		Transform item = mGrid.GetChild(index);
		if(item == null || point == null) return;
		item.transform.FindChild("UILabel_SpeedScale").GetComponent<UILabel>().text = point.speedScale.ToString();
		item.transform.FindChild("UILabel_Rchange").GetComponent<UILabel>().text = point.rChange.ToString();
		item.transform.FindChild("UILabel_During").GetComponent<UILabel>().text = point.time.ToString();
	}

	public void clear()
	{
		int index = 0;
		List<Transform> itemlist = mGrid.GetChildList();

		foreach(Transform item in itemlist)
		{
			GameObject.DestroyImmediate(item.gameObject);
		}
	}
}
