﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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






		//フェードが完全に終わったら次のシーンへ
		Application.LoadLevel("Main");

	}
}
