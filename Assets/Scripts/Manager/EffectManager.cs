using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonBase<EffectManager>{

	public List<GameObject> pEffectList;


	void Awake()
	{
		if (this != GetInstance)
		{
			Destroy(this); 
		}

		//		DontDestroyOnLoad (this.gameObject);
	}

	public void EffectCreate(int _index,Vector3 _pos)
	{
		GameObject effectObject = (GameObject)Instantiate
		(
			pEffectList[_index],
			_pos,
			Quaternion.identity
		);
		

	}
}
