using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class GameTable {
	
	public string name;
	public int row = 0;
	public int col = 0;
	public List<FIELD_TYPE> type = new List<FIELD_TYPE>();
	
	public abstract void AddValue(int index, int type, string values);
	public abstract void Destroy();
	
	public static GameTable Clone(Type type)
	{
		GameTable tab = type.Assembly.CreateInstance( type.Name ) as GameTable;
		return tab;
	}
}



#region FishTable
public class TableFish
	: GameTable
{
	private enum Table_Fish_Field
	{
		ID = 0,
		CHINESENAME = 1,
		NAME = 2,
		MOVE = 3,
		DEAD = 4,
		HIT = 5,
		SPEED = 6,
		POINT = 7,
		Width = 8,
		Height = 9,
		ScaleFactor
	}
	public class FishRecord
	{
		public int fishId;
		public string chineseName;
		public string name;
		public string moveAnim;
		public string deadAnim;
		public string hitAnim;
		public float speed;
		public int value;
		public float width;
		public float height;
		public float scaleFactor;
	}
	
	public List<FishRecord> recordList = new List<FishRecord>();
	
	public override void Destroy()
	{
		recordList.Clear();
		recordList = null;
	}
	
	public override void AddValue (int index, int column, string values)
	{
		FishRecord record = null;
		if(recordList.Count<=index)
		{
			record = new FishRecord();
			recordList.Add(record);
		}
		else
			record = recordList[index];
		
		switch((Table_Fish_Field)column)
		{
		case Table_Fish_Field.ID:
			record.fishId = int.Parse(values);
			break;
		case Table_Fish_Field.CHINESENAME:
			record.chineseName = values;
			break;
		case Table_Fish_Field.NAME:
			record.name = values;
			break;
		case Table_Fish_Field.MOVE:
			record.moveAnim = values;
			break;
		case Table_Fish_Field.DEAD:
			record.deadAnim = values;
			break;
		case Table_Fish_Field.HIT:
			record.hitAnim = values;
			break;
		case Table_Fish_Field.SPEED:
			record.speed = float.Parse(values);
			break;
		case Table_Fish_Field.POINT:
			record.value = int.Parse(values);
			break;
		case Table_Fish_Field.Width:
			record.width = float.Parse(values);
			break;
		case Table_Fish_Field.Height:
			record.height = float.Parse(values);
			break;
		case Table_Fish_Field.ScaleFactor:
			record.scaleFactor = float.Parse(values);
			break;
		}
	}

	public FishRecord getRecordByFishKindId(int id)
	{
		foreach(FishRecord record in recordList)
		{
			if(record.fishId == id)
				return record;
		}
		return null;
	}

	public int getFishKindIdByName(string name)
	{
		foreach(FishRecord record in recordList)
		{
			if(name == record.name)
				return record.fishId;
		}
		return -1;
	}

	public FishRecord getRecordByName(string name)
	{
		foreach(FishRecord record in recordList)
		{
			if(name == record.name)
				return record;
		}
		return null;
	}
}
#endregion
