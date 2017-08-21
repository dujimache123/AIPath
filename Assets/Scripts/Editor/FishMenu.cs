using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class FishMenu
{
    [MenuItem("Fish/Open/Fish AI Creater")]
    static void OpenFishAICreater()
    {
        EditorWindow.GetWindow<AICreaterWindow>(false, "AI Creater", true).Show();
    }

    [MenuItem("Fish/Open/Fish AI Viewer")]
    static void OpenFishAIViewer()
    {
        EditorWindow.GetWindow<AIViewerWindow>(false, "AI Viewer", true).Show();
    }

    [MenuItem("Fish/Open/Fish Season Viewer")]
    static void OpenFishSeasonViewer()
    {
        EditorWindow.GetWindow<SeasonViewerWindow>(false, "Season Viewer", true).Show();
    }

    [MenuItem("Fish/Open/Fish Season Modifier")]
    static void OpenFishSeasonModifier()
    {
        EditorWindow.GetWindow<SeasonModifierWindow>(false, "Season Modifier", true).Show();
    }

	[MenuItem ("Fish/Open/Fish Prefab Toolbar")]
	static void OpenFishPrefabToolbar ()
	{
		EditorWindow.GetWindow<FishPrefabTool>(false, "Fish Prefabs", true).Show();
	}

	[MenuItem("Fish/Open/Reset Fish Prefab Toolbar")]
	static void onResetFishPrefabToolbar()
	{
		EditorWindow.GetWindow<FishPrefabTool>(false, "Fish Prefabs", true).Reset();

//		string[] files = Directory.GetFiles("Assets/Scripts/Editor/Preview/");
//
//		for(int i = 0; i < files.Length; i ++)
//		{
//			string extention = Path.GetExtension(files[i]);
//			if(extention != ".png") continue;
//			string filename = Path.GetFileNameWithoutExtension(files[i]);
//			Debug.Log(AssetDatabase.RenameAsset(files[i],"Prefab_Fish_" + filename.Split('_')[0]));
//		}
	}

	[MenuItem("Fish/Open/Create Fish Prefab Preview")]
	static void onCreateFishPrefabPreview()
	{
		string[] files = Directory.GetFiles("Assets/Scripts/Editor/Preview/");

		for(int i = 0; i < files.Length; i ++)
		{
			string extention = Path.GetExtension(files[i]);
			if(extention != ".png") continue;
			string filename = Path.GetFileNameWithoutExtension(files[i]);
            if (filename.Contains("Prefab_Fish")) continue;
			AssetDatabase.RenameAsset(files[i],"Prefab_Fish_" + filename.Split('_')[0]);
		}
	}

	[MenuItem("Fish/CreatePrefabs")]
	static void CreatePrefabs()
	{
		TableFish fishTable = (TableFish)GameTableManager.getInstance().GetTable("table_fish");
		foreach(Object selectobj in Selection.objects)
		{
			GameObject newobj = new GameObject();
			string fishname = selectobj.name.Split('_')[0];
			TableFish.FishRecord record = fishTable.getRecordByName(fishname);
			if(record == null) continue;
			newobj.name = "Prefab_Fish_" + fishname;
			UITexture uitex = newobj.AddComponent<UITexture>();
			uitex.mainTexture = (Texture)selectobj;
			uitex.MakePixelPerfect();
			uitex.transform.localScale = new Vector3(record.scaleFactor,record.scaleFactor,1);
			string prefabpath = "Assets" + "/Resources/FishPrefabs/" + newobj.name + ".prefab";
			PrefabUtility.CreatePrefab(prefabpath,newobj);
		}
	}

//	[MenuItem("Fish/CreateOneSeason")]
//	static void CreateOneSeason()
//	{
//		GameObject oneseaon = new GameObject();
//		oneseaon.name = "OneSeason";
//		UIRoot root = GameObject.FindObjectOfType<UIRoot>();
//		oneseaon.transform.parent = root.transform;
//		oneseaon.transform.localPosition = Vector3.zero;
//		oneseaon.transform.localScale = Vector3.one;
//	}
	[MenuItem("Fish/CreateFishSeasonInfo")]
	static void CreateFishSeasonInfo()
	{
		Transform anchorBL = GameObject.FindGameObjectWithTag("SeasonRoot").transform;
		Transform oneseason = anchorBL.FindChild("OneSeason");
		if(oneseason == null)
		{
			GameObject oneseasonObj = new GameObject();
			oneseasonObj.name = "OneSeason";
			oneseasonObj.transform.parent = anchorBL;
			oneseasonObj.transform.localPosition = Vector3.zero;
			oneseasonObj.transform.localScale = Vector3.one;
			oneseasonObj.layer = anchorBL.gameObject.layer;
			oneseason = oneseasonObj.transform;
			oneseason.gameObject.AddComponent<OneSeasonComponent>();
		}

		GameObject oneseaoninfo = new GameObject();
		oneseaoninfo.name = "OneSeasonInfo";
		oneseaoninfo.transform.parent = oneseason;
		oneseaoninfo.transform.localPosition = Vector3.zero;
		oneseaoninfo.transform.localScale = Vector3.one;
		oneseaoninfo.layer = oneseason.gameObject.layer;
		oneseaoninfo.AddComponent<FishSeasonInfoComponent>();


	}
	[MenuItem("Fish/Export Season")]
	static void ExportSeason()
	{
        Transform anchorBL = GameObject.FindGameObjectWithTag("SeasonRoot").transform;
		Transform oneseason = anchorBL.FindChild("OneSeason");
		if(oneseason != null)
		{
			oneseason.GetComponent<OneSeasonComponent>().exportXml("test.xml");
			AssetDatabase.Refresh();
			string sourcepath = "Assets/test.xml"; 
			string destpath = "Assets/Resources/Configs/" + "test.xml";
			AssetDatabase.DeleteAsset(destpath);
			AssetDatabase.MoveAsset(sourcepath,destpath);
			AssetDatabase.Refresh();
		}
	}

	[MenuItem("Fish/AddModifier")]
	static void addModifier()
	{
		Selection.activeGameObject.AddComponent<ModifyProperty>();
	}
}
