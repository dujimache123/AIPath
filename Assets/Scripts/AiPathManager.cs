using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class AiPathManager
{
	private List<FishPath> pathList;

	private static AiPathManager pathManager;

	private int currentPathIndex;

	private AiPathManager()
	{
        pathList = new List<FishPath>();
		currentPathIndex = -1;
	}

	public static AiPathManager getInstance()
	{
		if(pathManager == null)
		{
			pathManager = new AiPathManager();
		}

		return pathManager;
	}

	public void initialize()
	{
        pathList.Clear();
		TextAsset ta = (TextAsset)Resources.Load("Configs/table_ai");
		string[] allines = ta.text.Split('\n');

        FishPath currentPath = null;
		for(int i = 0; i < allines.Length; i ++)
		{
            if (allines[i].Length <= 1) continue;
			if(allines[i][0] == '#') continue;
			if(allines[i][0] == '0')	//新的路径
			{
                currentPath = ScriptableObject.CreateInstance<FishPath>();
				pathList.Add(currentPath);
			}
			string[] cells = allines[i].Split('\t');
            FishPathControlPoint keyPoint = ScriptableObject.CreateInstance<FishPathControlPoint>();
			//keyPoint.id = int.Parse(cells[0]);
			keyPoint.mRotationChange = float.Parse(cells[1]);
            keyPoint.mSpeedScale = float.Parse(cells[2]);
            keyPoint.rotateFactor = float.Parse(cells[3]);
            keyPoint.speedFactor = float.Parse(cells[4]);
            keyPoint.mTime = 0.5f;
            currentPath.AddPoint(keyPoint);
		}
	}

    public FishPath getPath(int index)
	{
		if(index >= 0 && index < pathList.Count)
			return pathList[index];
		return null;
	}

    private void saveFile(string filePath, string content)
    {
        FileStream fs = new FileStream(filePath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(content);
        sw.Flush();
        fs.Close();
    }

	public void saveIni(FishPath path)
	{
        if (null == path)
        {
            return;
        }
        string filePath = EditorUtility.SaveFilePanel("Save Path Ini", Application.dataPath + "/Resources/Pathes/", path.FileName + ".ini", "ini");
        if (filePath.Length != 0)
        {
            string content = string.Format("[AI{0}]\n", path.mPathId);
            int index = 0;
            string temp = "";
            foreach (FishPathControlPoint cp in path.controlPoints)
            {
                temp = string.Format("{0}=\"{1} {2} {3}\"\n",index,2,-cp.mRotationChange,cp.mSpeedScale);
                content += temp; 
                index ++;
            }
            this.saveFile(filePath, content);
        }
	}

    public void saveTab(FishPath path)
    {
        if (null == path)
        {
            return;
        }
        string filePath = EditorUtility.SaveFilePanel("Save Path Tab", Application.dataPath + "/Resources/Pathes/", path.FileName + ".bytes", "bytes");
        if (filePath.Length != 0)
        {
            string content = "";
            int index = 0;
            string temp = "";
            foreach (FishPathControlPoint cp in path.controlPoints)
            {
                temp = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", index, -cp.mRotationChange, cp.mSpeedScale, cp.rotateFactor, cp.speedFactor);
                content += temp;
                index++;
            }
            this.saveFile(filePath, content);
        }
    }
	public bool load(string filepath)
	{
		if(filepath == null || filepath.Length == 0)
		{
			Debug.Log("路径文件名不正确");
			return false;
		}
		if(File.Exists(filepath) == false)
		{
			Debug.Log("路径文件不存在");
			return false;
		}

		FileStream fs = new FileStream(filepath,FileMode.Open);
		StreamReader sr = new StreamReader(fs);
        FishPath currentPath = null;
		while (sr.Peek() >= 0) 
		{
			string oneline = sr.ReadLine();
			//Debug.Log(oneline);
			if(oneline[0] == '#') continue;
			if(oneline[0] == '0')	//新的路径
			{
                currentPath = ScriptableObject.CreateInstance<FishPath>();
				pathList.Add(currentPath);
			}
			string[] cells = oneline.Split('\t');
            FishPathControlPoint keyPoint = ScriptableObject.CreateInstance<FishPathControlPoint>();
			//keyPoint.id = int.Parse(cells[0]);
			keyPoint.mRotationChange = float.Parse(cells[1]);
            keyPoint.mSpeedScale = float.Parse(cells[2]);
			keyPoint.mTime = 0.5f;
            currentPath.AddPoint(keyPoint);
		}


		return true;
	}

	public int getPathCount()
	{
		return pathList.Count;
	}
}
