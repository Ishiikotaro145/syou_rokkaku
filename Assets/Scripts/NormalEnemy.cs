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
        hpBar.localScale = new Vector2(3f * currentHP / hp, 3f);
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