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

public class AICreaterWindow : EditorWindow
{
    static public AICreaterWindow instance;

	/// <summary>
	/// Draw the custom wizard.
	/// </summary>
    Vector2 mScroll = Vector2.zero;
    int selectIndex = 0;

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
        FishPath fishPath = pathRender.FishPathData;
        if (null == fishPath)
        {
            if (GUILayout.Button("创建新路径", GUILayout.Width(100)))
            {
                pathRender.FishPathData = new FishPath();
                fishPath = pathRender.FishPathData;
            }
            return;
        }
        
        
		NGUIEditorTools.DrawHeader("Control Points", true);
		{
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

            EditorGUILayout.BeginHorizontal();
            fishPath.mPathId = EditorGUILayout.IntField("PathId", fishPath.mPathId);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            if (GUILayout.Button("Load"))
            {
                string filepath = EditorUtility.OpenFilePanel("Load", Application.dataPath + "/Resources/Pathes/", "bytes");
                if (filepath.Length > 0)
                {
                    fishPath = AiPathManager.getInstance().loadOnePath(filepath);
                    pathRender.FishPathData = fishPath;
                    EditorUtility.SetDirty(Selection.activeGameObject);
                }
            }

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            
            
            if (GUILayout.Button("Save INI"))
            {
                AiPathManager.getInstance().saveIni(fishPath);
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Save TAB"))
            {
                AiPathManager.getInstance().saveTab(fishPath);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            if (GUILayout.Button("Reset Path"))
            {
                if (EditorUtility.DisplayDialog("Resetting path?", "Are you sure you want to delete all control points?", "Delete", "Cancel"))
                {

                    fishPath.ResetPath();
                    EditorUtility.SetDirty(Selection.activeGameObject);
                    return;
                }
            }

            if (selectIndex > -1 && selectIndex < fishPath.numberOfControlPoints)
            {
                FishPathControlPoint point = fishPath.controlPoints[selectIndex];
                if (point)
                {
                    point.highLight = GUILayout.Toggle(point.highLight, "HighLight");
                    point.color = EditorGUILayout.ColorField("Line Colour", point.color);

                    point.mTime = EditorGUILayout.FloatField("Time", point.mTime);
                    point.mSpeedScale = EditorGUILayout.FloatField("SpeedScale", point.mSpeedScale);
                    point.mRotationChange = EditorGUILayout.FloatField("RotationChange", point.mRotationChange);
                }
            }

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("添加控制点"))
            {
                fishPath.AddPoint();
                EditorUtility.SetDirty(pathRender);
            }
            EditorGUILayout.EndHorizontal();

            mScroll = GUILayout.BeginScrollView(mScroll);

			bool delete = false;
			
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
                if (GUILayout.Button("X", GUILayout.Width(22f)))
                {
                    fishPath.DeletePoint(i);
                    if (selectIndex == i)
                        selectIndex = -1;
                    i--;
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
            EditorApplication.MarkSceneDirty();
        }

	}

    void OnSelectionChange() { Repaint(); }
}
