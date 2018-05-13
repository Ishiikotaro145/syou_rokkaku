using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : EnemyBase
{
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
            GetComponent<CircleCollider2D>().enabled = false; 
            EnemyManager.GetInstance.TellDead(); 
            Destroy(gameObject);
        }

        isDamage = true;
        damageTime = 0;


        return true;
    }


    void Update()
    {
        //Debug.Log("NormalEnemyUpdate");
        Damage();
    }
}