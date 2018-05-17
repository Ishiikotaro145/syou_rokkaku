using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uGUInowStage : MonoBehaviour {

	public List<GameObject> nowStageList;



	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		int nowStage = StageManager.GetInstance.GetNowStage();

		for (int cnt = 0; cnt < nowStageList.Count; cnt++)
		{
			Image numImage = nowStageList [cnt].GetComponent<Image> ();
			var color = numImage.color;
			color.a = 0.0f;
			numImage.color = color;
		}
		Image activeImage = nowStageList [nowStage].GetComponent<Image> ();
		var activeColor = activeImage.color;
		activeColor.a = 1.0f;
		activeImage.color = activeColor;

	}
}
