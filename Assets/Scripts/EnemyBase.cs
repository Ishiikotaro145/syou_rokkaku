using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //エネミーのパラメータ
    public int hp; //敵のHP
    public bool isRefrect; //反射するか
    public int exp;
    int enemyID;


    //演出周りで必要な変数
    bool isSpawnAnime = false; //登場アニメ
    bool isDeadAnime = false; //死亡アニメ

    //アニメーションが無いため仮　それっぽく魅せる演出用の変数
    float addScale = 3.0f;
    float alpha = 1.0f;
    private CircleCollider2D _circleCollider2D;

    // Use this for initialization
    void Start()
    {
        isSpawnAnime = true;

        //マネージャーにスポーンしたことを伝える
        EnemyManager.GetInstance.TellSpawn();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAnime();

        DeadAnime();
    }


    protected void SpawnAnime()
    {
        if (isSpawnAnime == false) return;

        //
        addScale *= 0.9f;
        transform.localScale = new Vector3(2.0f + addScale, 2.0f + addScale, 2.0f);

        if (addScale > 0.05f) return;
        isSpawnAnime = false;
    }


    protected void DeadAnime()
    {
        if (isDeadAnime == false) return;

        addScale *= 0.9f;
        transform.localScale = new Vector3(addScale, addScale, 1.0f);
        //死亡アニメが終わったらデリートする

        if (addScale > 0.05f) return;

        isDeadAnime = false;
        gameObject.active = false;
    }


    public void HitByPlayer()
    {
        //斬撃エフェクトを出す。

        Debug.Log("Damage");
        //HP減らす処理
        hp--;
        if (hp <= 0)
        {
            _circleCollider2D.enabled = false;
            isDeadAnime = true;
            EnemyManager.GetInstance.TellDead();
            addScale = 1.0f;
        }
    }
}