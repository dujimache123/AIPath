//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

/// <summary>
/// Tool that makes it easy to drag prefabs into it to "cache" them for ease of use.
/// </summary>

public class AIViewerWindow : EditorWindow
{
    static public AIViewerWindow instance;

	/// <summary>
	/// Draw the custom wizard.
	/// </summary>
    Vector2 mScroll = Vector2.zero;
    int selectIndex = 0;
    int selectedPathId = 1;
    string[] names = {};
    int[] values = {};
    void OnEnable() { instance = this; }
    void OnDisable() { instance = null; }

	void OnGUI ()
	{
        GameObject activeGameObject = Selection.activeGameObject;
        if (null == activeGameObject)
        {
            return;
        }
        PathRender pathRender = activeGameObject.GetComponent<PathRender>();
        if (null == pathRender)
        {
            return;
        }

        if (GUILayout.Button("加载路径文件", GUILayout.Width(100)))
        {
            string filePath = EditorUtility.OpenFilePanel("Open Ai Bytes", Application.dataPath + "/Resources/Pathes/", "bytes");

            if (filePath.Length > 0)
            {
                selectedPathId = 0;
                AiPathManager.getInstance().initialize(filePath);
            }
        }

        int pathCnt = AiPathManager.getInstance().getPathCount();
        if (pathCnt == 0)
        {
            return;
        }
        selectedPathId = EditorGUILayout.IntSlider("AI index",selectedPathId, 0, pathCnt);
        
        FishPath fishPath = pathRender.FishPathData;

        if (GUILayout.Button("更新路径", GUILayout.Width(100)))
        {
            pathRender.FishPathData = AiPathManager.getInstance().getPath(selectedPathId);
            fishPath = pathRender.FishPathData;
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
        
		NGUIEditorTools.DrawHeader("Control Points", true);
		{
            if (null == fishPath) 
                return;
			GUILayout.BeginHorizontal();
			GUILayout.Space(3f);
			GUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            fishPath.renderPath = EditorGUILayout.Toggle("Render Path", fishPath.renderPath);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            fishPath.lineColor = EditorGUILayout.ColorField("Line Color", fishPath.lineColor);
            GUILayout.Space(5);


            EditorGUILayout.BeginHorizontal();
            fishPath.baseSpeed = EditorGUILayout.FloatField("Speed", fishPath.baseSpeed);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            if (GUILayout.Button("Load"))
            {
                string filepath = EditorUtility.OpenFilePanel("Load", Application.dataPath + "/Resources/Pathes/", "bytes");
                if (filepath.Length > 0)
                {
                    //fish.FishPathData = PathConfigManager.GetInstance().Load(filepath);
                    //fish.FishPathData.FileName = Path.GetFileName(filepath);
                    EditorUtility.SetDirty(Selection.activeGameObject);
                }
            }

            mScroll = GUILayout.BeginScrollView(mScroll);
            for (int i = 0; i < fishPath.numberOfControlPoints; i ++)
            {
                GUI.backgroundColor = selectIndex == i ? Color.white : new Color(0.8f, 0.8f, 0.8f);
                GUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f));
                GUILayout.Space(5f);
                GUILayout.Label(i.ToString(), GUILayout.Width(24f));
                if (GUILayout.Button("hello", "OL TextField", GUILayout.Height(25f)))
                {
                    selectIndex = i;
                    fishPath.SelectedLineIndex = selectIndex;
                }
                GUILayout.EndHorizontal();
            }
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(3f);
			GUILayout.EndHorizontal();
		}

        if (GUI.changed)
        {
            fishPath.CaculateFinePoints();
            EditorUtility.SetDirty(Selection.activeGameObject);
        }

	}

    void OnSelectionChange() { Repaint(); }
}
