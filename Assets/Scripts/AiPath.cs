using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class AiPoint
{
	public int id;
	public double speedScale;
	public double rChange;
	public double time;
	public double rotateFactor;
	public double speedFactor;
	public AiPoint()
	{

	}

	public AiPoint(int id, double speedScale, double rChange,double time)
	{
	    this.id = id;
	    this.speedScale = speedScale;
	    this.rChange = rChange;
	    this.time = time;
	}

	public override string ToString()
	{
	      return string.Format("{0:d}  {1:f}   {2:f}    {3:f}", this.id, this.speedScale, this.rChange,this.time);
	}
}

public class AIPath
{
	public double bornX;
	public double bornY;
	public double bornSpeed;
	public double bornR;
	public List<AiPoint> aiPoints;
	public double secondPerFrame;
	public AIPath()
	{
		this.bornX = 0;
		this.bornY = 360;
		this.bornSpeed = 80;
		this.bornR = 0;
		this.secondPerFrame = 0.03f;
	    this.aiPoints = new List<AiPoint>();
	}
	public AIPath(double bornx, double borny, double bornspeed, double bornr)
	{
	    this.bornX = bornx;
	    this.bornY = borny;
	    this.bornSpeed = bornspeed;
	    this.bornR = bornr;
	    this.secondPerFrame = 0.03f;
	    this.aiPoints = new List<AiPoint>();
	}

	public void addPoint(AiPoint p)
	{
	    if (p != null)
	    {
	        this.aiPoints.Add(p);
	    }
	}

	public void removePoint(int index)
	{
		if(this.aiPoints == null)return;
		if(index >-1 && index < this.aiPoints.Count)
			this.aiPoints.RemoveAt(index);
	}

	public void updatePoint(int index,double speedscale,double rchange,double timeduring)
	{
		if(this.aiPoints == null)return;
		if(index >-1 && index < this.aiPoints.Count)
		{
			this.aiPoints[index].speedScale = speedscale;
			this.aiPoints[index].rChange = rchange;
			this.aiPoints[index].time = timeduring;
		}
	}

	public AiPoint getPoint(int index)
	{
		if(index >-1 && index < this.aiPoints.Count)
		{
			return this.aiPoints[index];
		}
		return null;
	}

	public int getPathPointCnt()
	{
		return aiPoints.Count;
	}

	//获取插值后的精细路径点
	public List<Vector3> getFinePoints()
	{
	    List<Vector3> list = new List<Vector3>();
		list.Add(new Vector2((float)this.bornX, (float)this.bornY));
		Vector2 lastpoint = new Vector2((float)this.bornX, (float)this.bornY);
	    double currentR = 0;
	    if (this.aiPoints.Count == 0)
	    {
	        list.Add(lastpoint);
	    }
	    foreach (AiPoint p in this.aiPoints)
	    {
	        double time = 0;
	        double rspeed = (p.rChange) / p.time;
	        double aispeed = p.speedScale * this.bornSpeed;
			Vector3 nextPoint = new Vector3();
	        while (true)
	        {
	            time += this.secondPerFrame;
	            if (time > p.time)
	            {
					double dt = p.time - (time - secondPerFrame);
					nextPoint = new Vector3();
					nextPoint.x = lastpoint.x + (float)(aispeed * Math.Cos(currentR*Mathf.Deg2Rad) * dt);
					nextPoint.y = lastpoint.y + (float)(aispeed * Math.Sin(currentR*Mathf.Deg2Rad) * dt);
					nextPoint.z = (float)currentR;
					list.Add(nextPoint);
					lastpoint = nextPoint;
					currentR += (rspeed*dt);
					//Debug.Log(currentR);
	                break;
	            }
	            nextPoint = new Vector3();
	            nextPoint.x = lastpoint.x + (float)(aispeed * Math.Cos(currentR*Mathf.Deg2Rad) * this.secondPerFrame);
				nextPoint.y = lastpoint.y + (float)(aispeed * Math.Sin(currentR*Mathf.Deg2Rad) * this.secondPerFrame);
				nextPoint.z = (float)currentR;
	            list.Add(nextPoint);
	            lastpoint = nextPoint;
	            currentR += (rspeed*this.secondPerFrame);
				//Debug.Log(currentR);
	        }
	    }
		//if(list.Count % 2 != 0)
		//	list.RemoveAt(list.Count - 1);
	    return list;
	}

	public void save(string filename)
	{
		//string json = JsonMapper.ToJson(this);
		FileStream fs = new FileStream(filename,FileMode.CreateNew);
		StreamWriter sw = new StreamWriter(fs);
		//sw.Write(json);
		foreach(AiPoint point in this.aiPoints)
		{

		}

		sw.Flush();
		fs.Close();
	}

	public static AIPath loadFromJson(string filename)
	{
		FileStream fs = new FileStream(filename,FileMode.Open);
		StreamReader sw = new StreamReader(fs);
		string jsonstr = sw.ReadToEnd();
		AIPath path = JsonMapper.ToObject<AIPath>(jsonstr);
		return path;
	}
}
