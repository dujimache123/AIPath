using UnityEngine;
using System.Collections;
using LitJson;

public class PathControl : MonoBehaviour {
	public UIInput mTextBox_BornX;
	public UIInput mTextBox_BornY;
	public UIInput mTextBox_BornR;
	public UIInput mTextBox_BornSpeed;
	public UIInput mTextBox_SpeedScale;
	public UIInput mTextBox_RChange;
	public UIInput mTextBox_During;
	public AIPointListPanel mPointListPanel;
	private PathRender mPathRender;
	private FishPath mAiPath;
	private int pathIndex = -1;
	private int pointindex = 0;

	// Use this for initialization
	void Start () {
		mAiPath = null;
		mPathRender = this.GetComponent<PathRender>();
	}
	
	// Update is called once per frame
	void Update () {
		//print (Time.deltaTime);
	}

	public void onClickLoadPath()
	{
		AiPathManager.getInstance().load(@"E:\Git\quick-cocos2d-x-2.2.5-plus-release\project\framework\module\fishing\res\config\table_ai.tab");
		onClickNextPath();
		return;
		string filepath = FileDialogDLL.showOpenFileDialog();
		if(filepath.Length > 0)
		{
			//this.mAiPath = AIPath.loadFromJson(filepath);
			//updateItemList();

		}
	}

	public void onClickSavePath()
	{
		string filepath = FileDialogDLL.showSaveFileDialog();
		if(filepath.Length > 0)
		{
			print (filepath);
			//this.mAiPath.save(filepath);
		}
	}

	public void onClickPrePath()
	{
		if(pathIndex > 0)
		{
			pathIndex -= 1;
			this.mAiPath = AiPathManager.getInstance().getPath(pathIndex);
			mPointListPanel.clear();
		}
	}

	public void onClickNextPath()
	{
		if(pathIndex < AiPathManager.getInstance().getPathCount() - 1)
		{
			pathIndex += 1;
			this.mAiPath = AiPathManager.getInstance().getPath(pathIndex);
			mPointListPanel.clear();
		}
	}
}
