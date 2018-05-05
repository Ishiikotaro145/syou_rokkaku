using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	public void ButtonTap(int _Stage)
	{
		//フェード処理 
		PlayerPrefs.SetInt("StageSelect",_Stage);




		//フェードが完全に終わったら次のシーンへ
		Application.LoadLevel("Main");

	}
}
