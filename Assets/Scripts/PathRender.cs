using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathRender : MonoBehaviour {
    //[HideInInspector]
    [SerializeField]
    private FishPath mFishPath;
	private UISpriteAnimation fish;
	private float mFishLife;
	private int mPosIndex = 0;
	List<FinePoint> pathPointList;

    public FishPath FishPathData
    {
        get { return mFishPath; }
        set { mFishPath = value; }
    }

	// Use this for initialization
	void Start ()
	{
		mFishLife = 0;
        mFishPath.CaculateFinePoints(transform);
	}

    public void OnDrawGizmos()
    {
        if (null == mFishPath)
        {
            return;
        }
        pathPointList = mFishPath.FinePointList;
        if (mFishPath && mFishPath.renderPath)
        {
            Vector2 vec2Pos = new Vector2(transform.localPosition.x, transform.localPosition.y);
            if (mFishPath.StartPosition != vec2Pos || pathPointList.Count == 0 || mFishPath.StartRotation != transform.eulerAngles.z)
            {
                if (Application.isPlaying == false)
                    mFishPath.CaculateFinePoints(transform);
            }
            for (int i = 0; i < pathPointList.Count - 1; i++)
            {
                try
                {
                    if (mFishPath.controlPoints[pathPointList[i].controlIndex].highLight)
                        Gizmos.color = mFishPath.controlPoints[pathPointList[i].controlIndex].color;
                    else
                        Gizmos.color = mFishPath.lineColor;
                    if (mFishPath.SelectedLineIndex == pathPointList[i].controlIndex)
                        Gizmos.color = Color.red;
                    Gizmos.DrawLine(pathPointList[i].position * 0.002777778f, pathPointList[i + 1].position * 0.002777778f);
                }
                catch
                {
                    //Debug.Log("wzw");
                }

            }
        }
    }

	// Update is called once per frame
	void Update () {
        //if (mFishPath != null)
        //{
        //    mPosIndex = (int)(mFishLife / 0.03f);
        //    if (mPosIndex >= pathPointList.Count)
        //    {
        //        mPosIndex = 0;
        //        mFishLife = 0;
        //    }
        //    fish.transform.localPosition = new Vector3(pathPointList[mPosIndex].x - 640, pathPointList[mPosIndex].y - 360, 0);
        //    fish.transform.localRotation = Quaternion.Euler(0, 0, (float)mFishPath.bornR);
        //    mFishLife += Time.deltaTime;
        //}
	}
}
