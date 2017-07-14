using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

public enum FIELD_TYPE
{
	EFT_NUM = 0,
	EFT_FLOAT = 1,
	EFT_TEXT = 2
}

public class FIELD
{
	public FIELD_TYPE FieldType;
	public string FieldValueStr;
}

public class GameTableManager
{
	Dictionary<string, GameTable>	gameTable = new Dictionary<string, GameTable>();
	private static GameTableManager mInstance;
	private GameTableManager()
	{

	}
	public static GameTableManager getInstance()
	{
		if(mInstance == null)
		{
			mInstance = new GameTableManager();
			mInstance.initialize();
		}
		return mInstance;
	}
	public void initialize()
	{
		gameTable.Clear();
		LoadTable("table_fish",typeof(TableFish));
	}

	
	private bool LoadTable(string tablename, Type type)
	{
		if(gameTable.ContainsKey(tablename) == true)
		{
			Debug.Log("Load table is error!!! Had a file! Name="+tablename);
			return false;
		}

		TextAsset o = Resources.Load("Configs/" + tablename) as TextAsset;

		if(o == null)
		{
			Debug.Log("Load table is error!!!Read File is error! File="+tablename);
			return false;
		}
		
		GameTable tab = GameTable.Clone(type);
		tab.name = o.name;
		
		ReadTable(o.bytes, ref tab);
		
		gameTable.Add(tab.name, tab);
		return true;
	}
	
	public void ReadTable(byte[] bytes, ref GameTable tab)
	{
		if(tab == null)
			return;
		string strAll = Encoding.UTF8.GetString(bytes);
		string[] lines = strAll.Split('\n');

		int row = 0;
		for(int index=0; index<lines.Length; index++)
		{
			//第一行为表头
			if(index == 0)
			{
				continue;
			}
			string str = lines[index];
			//空行或者注释行
			if (str.Length<1 || str[0]=='#')
				continue;

			string[] array = str.Split('\t');
			
			if(array.Length == 0)
				continue;
			
			for(int ndx=0; ndx<array.Length; ndx++)
			{
				string strField = array[ndx];
				tab.AddValue(row, ndx, strField.Trim());
			}
			row ++;
		}
		tab.row = row;
	}
	
	public void Destroy()
	{
		foreach(GameTable tab in gameTable.Values)
		{
			tab.Destroy();
		}
		
		gameTable.Clear();
		gameTable = null;
	}
	
	public GameTable GetTable(string strName)
	{
		GameTable tab = null;
		gameTable.TryGetValue(strName, out tab);
		return tab;
	}
	
	public void AddTable(GameTable tab)
	{
		gameTable.Add(tab.name, tab);
	}
}
