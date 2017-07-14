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

public class SeasonViewerWindow : EditorWindow
{
    static public SeasonViewerWindow instance;

	/// <summary>
	/// Draw the custom wizard.
	/// </summary>
    Vector2 mScroll = Vector2.zero;
    int selectIndex = 0;
    int selectedId = 0;
    string[] names = {};
    int[] values = {};
    void OnEnable() { instance = this; }
    void OnDisable() { instance = null; }

	void OnGUI ()
	{
        if (false == Application.isPlaying)
        {
            return;
        }
        

            if (GUILayout.Button("加载鱼汛文件", GUILayout.Width(100)))
            {
                string filePath = EditorUtility.OpenFilePanel("Open Season Xml", Application.dataPath + "/Resources/Configs/", "xml");

                if (filePath.Length > 0)
                {
                    selectedId = 0;
                    FishConfigManager.getInstance().loadFishSeasonConfig(filePath);
                }
            }

        int seasonCnt = FishConfigManager.getInstance().getSeasonCnt();
        if (seasonCnt == 0) return;
        selectedId = EditorGUILayout.IntSlider("Season Id", selectedId, 0, seasonCnt);

        if (seasonCnt > 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("上一个"))
            {
                selectedId--;
                selectedId = selectedId >= 0 ? selectedId : seasonCnt - 1;
                Transform fishRoot = GameObject.FindGameObjectWithTag("FishRoot").transform;
                int childCnt = fishRoot.childCount;
                for (int i = 0; i < childCnt; )
                {
                    Transform child = fishRoot.GetChild(i);
                    GameObject.DestroyImmediate(child.gameObject);
                    childCnt = fishRoot.childCount;
                }
                EventManager.getInstance().onEventFishSeason(selectedId);
            }
            GUILayout.Space(5);
            if (GUILayout.Button("下一个"))
            {
                selectedId++;
                selectedId = selectedId < seasonCnt ? selectedId : 0;
                Transform fishRoot = GameObject.FindGameObjectWithTag("FishRoot").transform;
                int childCnt = fishRoot.childCount;
                for (int i = 0; i < childCnt; )
                {
                    Transform child = fishRoot.GetChild(i);
                    GameObject.DestroyImmediate(child.gameObject);
                    childCnt = fishRoot.childCount;
                }
                EventManager.getInstance().onEventFishSeason(selectedId);
            }
            EditorGUILayout.EndHorizontal();
        }
	}

    void OnSelectionChange() { Repaint(); }
}
