using UnityEngine;
using System.Collections;

public enum FishStatus
{
	Status_UnActive = 0,
	Status_Active,
	Status_Dead
}

public class Fish : MonoBehaviour {
	public float mDelayActiveTime;
	float mCurrentLifeTime;
	float mLastFrameLifeTime;
	int mCurAiStep,mLastAiStep;
	float mCurSpeed;
    float mBaseSpeed;
	float mCurRotation;
	FishStatus mStatus;
	FishPath mFishPath;
	float AISTEP = 0.5f;
	float SECOND_ONE_FRAME = 0.03f;
	bool mIsNeedRoolUI_ = false;
	float mPosX,mPosY;
	float mFishWidth = 0;

	public float DelayActiveTime
	{
		get{return mDelayActiveTime;}
		set{mDelayActiveTime = value;}
	}

	public float CurrentLifeTime
	{
		get{return mCurrentLifeTime;}
		set{mCurrentLifeTime = value;}
	}

    public FishPath AiPath
	{
		get{return mFishPath;}
		set{mFishPath = value;}
	}

	public float Rotation
	{
		get{return mCurRotation;}
		set{mCurRotation = value;}
	}

	public float FishWidth
	{
		get{return mFishWidth;}
		set{mFishWidth = value;}
	}

    public float BaseSpeed
    {
        get { return this.mBaseSpeed; }
        set { this.mBaseSpeed = value; }
    }

	// Use this for initialization
	void Start () {
		//mDelayActiveTime = float.MaxValue;
		//mCurrentLifeTime = 0;
		mStatus = FishStatus.Status_UnActive;
		transform.Rotate(0,0,mCurRotation);
		mCurRotation = Mathf.Deg2Rad*mCurRotation;
		if(Mathf.Cos (mCurRotation) < 0)
		{
			gameObject.GetComponent<UITexture>().flip = UIBasicSprite.Flip.Vertically;
		}

		mPosX = transform.localPosition.x;
		mPosY = transform.localPosition.y;

        PathRender pathRender = this.GetComponent<PathRender>();
        if (pathRender)
        {
            mFishPath = pathRender.FishPathData;
            mBaseSpeed = mFishPath.baseSpeed;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(mStatus == FishStatus.Status_UnActive)
		{
			mDelayActiveTime -= Time.deltaTime;
			if(mDelayActiveTime <= 0)
				updateStatus(FishStatus.Status_Active);
		}
		else if(mStatus == FishStatus.Status_Active)
		{
			this.mLastFrameLifeTime = mCurrentLifeTime;
			mCurrentLifeTime += Time.deltaTime;
			mCurAiStep = Mathf.FloorToInt(mCurrentLifeTime / AISTEP);

			if(this.mLastAiStep != mCurAiStep)
			{
				//上一帧鱼life到当前帧aiStep时间的时间差。
				float stepsTimeBeforeCurretStep = this.CurrentLifeTime * AISTEP - this.mLastFrameLifeTime;
				int tmpStep = this.mLastAiStep;
				float t1 = this.mLastFrameLifeTime;
				while(true)
				{
					tmpStep = tmpStep + 1;
					if(tmpStep > this.mCurAiStep)
						break;
					float tmpDt = tmpStep * AISTEP - t1;
					t1 = tmpStep * AISTEP;
					//tempdt时间内用tempStep - 1的数值
					int cnt = Mathf.FloorToInt(tmpDt / SECOND_ONE_FRAME);
					for (int i = 0; i < cnt ; i ++ )
					{
						this.updateFishPositionAndRotation(tmpStep-1, SECOND_ONE_FRAME);
					}

					this.updateFishPositionAndRotation(tmpStep-1, tmpDt - SECOND_ONE_FRAME*cnt);
				}
				float curStepT = this.mCurrentLifeTime - this.mCurAiStep * AISTEP;
				int cnt1 = Mathf.FloorToInt(curStepT / SECOND_ONE_FRAME);
				for (int i = 0; i < cnt1;i ++)
				{
					this.updateFishPositionAndRotation(this.mCurAiStep, SECOND_ONE_FRAME);
				}
				this.updateFishPositionAndRotation(this.mCurAiStep, curStepT - SECOND_ONE_FRAME*cnt1);
				this.mLastAiStep = this.mCurAiStep;
			}
			else
			{
				int cnt1 = Mathf.FloorToInt(Time.deltaTime / SECOND_ONE_FRAME);
				for (int i = 0; i < cnt1; i ++)
				{
					this.updateFishPositionAndRotation(this.mCurAiStep, SECOND_ONE_FRAME);
				}
				this.updateFishPositionAndRotation(this.mCurAiStep, Time.deltaTime - SECOND_ONE_FRAME*cnt1);
			}
		}

        //if (this.checkFishOutOfScreen())
        //    this.gameObject.SetActive(false);
	}

	void updateStatus(FishStatus status)
	{
		mStatus = status;
	}

	void updateFishPositionAndRotation(int aiStep,float dt)
	{
        float lastSpeed = this.mBaseSpeed;
		float angleFactor = 0;
		float speedFactor = 0;
		if(this.mFishPath == null) return;
        int airecordcnt = this.mFishPath.numberOfControlPoints;
		if (aiStep >= 0 && aiStep < airecordcnt)
		{
            FishPathControlPoint curRecord = this.mFishPath.controlPoints[aiStep];
			if(curRecord == null) return;
            FishPathControlPoint lastRecord = null;
			if(aiStep > 0)
			{
                lastRecord = this.mFishPath.controlPoints[aiStep-1];
			}
			//根据玩家炮台位置转换坐标
			if(this.mIsNeedRoolUI_)
				angleFactor = - (float)curRecord.rotateFactor;
			else
				angleFactor = (float)curRecord.rotateFactor;

            this.mCurSpeed = (float)curRecord.speedFactor * this.mBaseSpeed;
            this.mCurRotation = this.mCurRotation + angleFactor * dt;
		}

		mPosX = mPosX + mCurSpeed * dt * Mathf.Cos(this.mCurRotation);
		mPosY = mPosY + mCurSpeed * dt * Mathf.Sin(-this.mCurRotation);

		this.transform.localPosition = new Vector3(mPosX,mPosY,0);			
		Vector3 eulerAngles = this.transform.eulerAngles;
		this.transform.eulerAngles = new Vector3(eulerAngles.x,eulerAngles.y,-this.mCurRotation * 57.29577951f);
		//print ("wzw " + aiStep + "   " + angleFactor);
	}

	bool checkFishOutOfScreen()
	{
		bool isOutOfScreen = false;
        if (mStatus == FishStatus.Status_Active)
		{
			if(mPosX > 1280 + mFishWidth || mPosX < -mFishWidth || mPosY > 720 + mFishWidth || mPosY < -mFishWidth)
				isOutOfScreen = true;
		}

		return isOutOfScreen;
	}
}
