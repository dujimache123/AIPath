using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using FileDialogTool;

//[DllImport ("FileDialogTool")]
public class FileDialogDLL : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static string showOpenFileDialog()
	{
		FileDialogToolClass dialog = new FileDialogToolClass();
		return dialog.showOpenFileDialog();
	}

	public static string showSaveFileDialog()
	{
		FileDialogToolClass dialog = new FileDialogToolClass();
		return dialog.showSaveFileDialog();
	}
}
