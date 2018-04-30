using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBase<EnemyManager> {

	int nowEnemyCnt = 0;//画面上にいるエネミーの総数


	public int GetNowEnemyCnt(){ return nowEnemyCnt; }



	void Awake()
	{
		if (this != GetInstance)
		{
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (this.gameObject);
	}


	public void TellSpawn()
	{
		nowEnemyCnt++;
	}


	public void TellDead()
	{
		nowEnemyCnt--;
	}



}
