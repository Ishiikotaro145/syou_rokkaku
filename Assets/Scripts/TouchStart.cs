using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchStart : MonoBehaviour {

	SpriteRenderer sprite;
	float alpha = 0.5f;
	bool isAdd = true;

	// Use this for initialization
	void Start () 
	{
		sprite = transform.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isAdd == true)
		{
			alpha += 0.02f;
			if(alpha >= 1.0f)isAdd = false;
		}
		else
		{
			alpha -= 0.02f;
			if(alpha <= 0.5f)isAdd = true;
		}
		var color = sprite.color;
		color.a = alpha;
		sprite.color = color;
	}
}
