﻿using UnityEngine;
using System.Collections;
using System;

public class FishPathControlPoint:ScriptableObject
{
	public float mTime;
	public float mSpeedScale;
	public float mRotationChange;	//x,y
    public float rotateFactor;
    public float speedFactor;
	public bool highLight;
	public Color color;

    public FishPathControlPoint(float time, float speedscale, float rchange, bool highlight, Color color)
	{
		mTime = time;
		mSpeedScale = speedscale;
		mRotationChange = rchange;
		highLight = highlight;
		this.color = color;
	}


    public FishPathControlPoint()
	{
		mTime = 0.5f;
		mSpeedScale = 1;
        mRotationChange = 0;
		highLight = false;
		this.color = Color.red;
	}
}
