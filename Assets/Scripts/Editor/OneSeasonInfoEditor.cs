using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FishSeasonInfoComponent))]
public class OneSeasonInfoEditor : Editor
{
	public override void OnInspectorGUI()
	{
		FishSeasonInfoComponent seasonInfoProperty = (FishSeasonInfoComponent)target;
		seasonInfoProperty.centerPoint = EditorGUILayout.Vector2Field("CenterPoint",seasonInfoProperty.centerPoint);

		seasonInfoProperty.speed = EditorGUILayout.FloatField("Speed", seasonInfoProperty.speed);
		seasonInfoProperty.aiId = EditorGUILayout.IntField("AiId",seasonInfoProperty.aiId);
		seasonInfoProperty.angle = EditorGUILayout.FloatField("Angle(Degree)",seasonInfoProperty.angle);

		seasonInfoProperty.transform.localPosition = new Vector3(seasonInfoProperty.centerPoint.x,-(seasonInfoProperty.centerPoint.y),0);
		//seasonInfoProperty.transform.eulerAngles = new Vector3(0,0,-seasonInfoProperty.angle);
	}
}
