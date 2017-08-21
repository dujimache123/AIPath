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

public class SeasonModifierWindow : EditorWindow
{
    static public SeasonModifierWindow instance;

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
        if (Application.isPlaying)
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
                Transform seasonRoot = GameObject.FindGameObjectWithTag("SeasonRoot").transform;

                int childCnt = seasonRoot.childCount;
                for (int i = 0; i < childCnt; )
                {
                    Transform child = seasonRoot.GetChild(i);
                    GameObject.DestroyImmediate(child.gameObject);
                    childCnt = seasonRoot.childCount;
                }
                EventManager.getInstance().onEventFishSeason(selectedId, true);
            }
            GUILayout.Space(5);
            if (GUILayout.Button("下一个"))
            {
                selectedId++;
                selectedId = selectedId < seasonCnt ? selectedId : 0;
                Transform seasonRoot = GameObject.FindGameObjectWithTag("SeasonRoot").transform;

                int childCnt = seasonRoot.childCount;
                for (int i = 0; i < childCnt; )
                {
                    Transform child = seasonRoot.GetChild(i);
                    GameObject.DestroyImmediate(child.gameObject);
                    childCnt = seasonRoot.childCount;
                }
                EventManager.getInstance().onEventFishSeason(selectedId, true);
            }
            EditorGUILayout.EndHorizontal();
        }
	}

    void OnSelectionChange() { Repaint(); }
}
