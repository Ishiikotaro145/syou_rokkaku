using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassEnemy : EnemyBase 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public override bool HitByPlayer(Vector2 speed)
    {
        if (!startFinish) return false;
        //斬撃エフェクトを出す。

//        Debug.Log("Damage");

        //HP減らす処理 
        currentHP--;
        hpBar.localScale = new Vector2(0.08f * currentHP / hp, 0.12f);
        if (currentHP == 0)
        {
        	
        }

        isDamage = true;
        damageTime = 0;


        return true;
    }
}
