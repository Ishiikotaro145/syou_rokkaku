using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //エネミーのパラメータ
    public int hp; //敵のHP

//    public bool isRefrect; //反射するか
    public int exp;
    int enemyID;
    private int currentHP;
    private Transform hpBar;
    private bool startFinish;
    private bool isPassable;
    private CircleCollider2D _collider2D;


    //演出周りで必要な変数
    bool isSpawnAnime = false; //登場アニメ
    bool isDeadAnime = false; //死亡アニメ

    //アニメーションが無いため仮　それっぽく魅せる演出用の変数
    float addScale = 3.0f;
    float alpha = 1.0f;

    void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
        isPassable = _collider2D.isTrigger;
    }
    
    // Use this for initialization
    void Start()
    {
        isSpawnAnime = true;
         
        //マネージャーにスポーンしたことを伝える
        EnemyManager.GetInstance.TellSpawn();
        currentHP = hp;
        hpBar = transform.GetChild(1);
        if (hp == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
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
//        //
//        addScale *= 0.8f;
//        transform.localScale = new Vector3(1.5f + addScale, 1.5f + addScale, 1.5f);
//
//        if (addScale > 0.05f) return;
        isSpawnAnime = false;
        startFinish = true;
    }


    protected void DeadAnime()
    {
        if (isDeadAnime == false) return;

//        addScale *= 0.9f;
//        transform.localScale = new Vector3(addScale, addScale, 1.0f);
//        //死亡アニメが終わったらデリートする
//
//        if (addScale > 0.05f) return;

        isDeadAnime = false;
//        gameObject.active = false;
        Destroy(gameObject);
    }


    public bool HitByPlayer()
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
            isDeadAnime = true;
            EnemyManager.GetInstance.TellDead();
            addScale = 1.0f;
        }

        return true;
    }

    public void TriggerPassableWhenNecessary()
    {
        if (isPassable) return;
        Debug.Log("Triggered "+name);
        _collider2D.isTrigger = !_collider2D.isTrigger;
    }
}