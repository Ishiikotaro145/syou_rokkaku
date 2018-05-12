using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBase<EnemyManager>
{
    int nowEnemyCnt = 0; //画面上にいるエネミーの総数


    public int GetNowEnemyCnt()
    {
        return nowEnemyCnt;
    }


    void Awake()
    {
        if (this != GetInstance)
        {
            Destroy(this); 
        }

//		DontDestroyOnLoad (this.gameObject);
    }


    void Update()
    {
        Debug.Log(nowEnemyCnt);
    }


    public void TellSpawn()
    {
        nowEnemyCnt++;
    }


    public void TellDead()
    {
        nowEnemyCnt--;
        if (nowEnemyCnt == 0)
        {
            UIScript.instance.WaveClear();
            StageManager.GetInstance.SetWaveClear();
        }
    }
}