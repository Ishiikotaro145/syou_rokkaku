using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : SingletonBase<ComboManager>{

	public float comboLimitTime;
	public Text comboText;
	public string Tanni;


	int nowCombo = 0;
	float limitTime = 0.0f;
	float alpha = 0.0f;
	bool isShowAlpha = false;
	int alphaInterval = 0;
	float addFontSize = 0;

	public void AddCombo()
	{ 
		nowCombo++;
		addFontSize = 30;
		limitTime = comboLimitTime;
		isShowAlpha = true;
		alpha = 1.0f;

		alphaInterval = 0;
	}


	void Awake()
	{
		if (this != GetInstance)
		{
			Destroy(this); 
		}

		//		DontDestroyOnLoad (this.gameObject);
	}


	// Use this for initialization
	void Start () 
	{
		
	}


	public void Init()
	{
		nowCombo = 0;
		limitTime = 0.0f;
		alpha = 0.0f;
		isShowAlpha = false;
	}


	// Update is called once per frame
	void Update () 
	{
		//コンボ文字を点滅させる。
		if (limitTime < (comboLimitTime / 3)) 
		{
			alphaInterval++;
			if (alphaInterval >= 10) 
			{
				isShowAlpha = !isShowAlpha;
				alphaInterval = 0;
			}

			if (isShowAlpha == true && limitTime >= 0.0f) 
			{
				alpha = 1.0f;
			}
			else
			{
				alpha = 0.0f;
			}
		}

		limitTime -= Time.deltaTime;
		if (limitTime >= 0.0f) 
		{
			
		}
		else
		{
			limitTime = 0.0f;
			nowCombo = 0;
			alpha = 0.0f;
			isShowAlpha = false;
		}



		//
		comboText.fontSize = 40 + (int)addFontSize;
		addFontSize *= 0.9f;

			
		var combocolor = comboText.color;
		combocolor.a = alpha;
		comboText.color = combocolor;
		comboText.text = nowCombo + Tanni;
		//Debug.Log ("nowCombo" + nowCombo);
	}
}