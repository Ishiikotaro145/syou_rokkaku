using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    //エネミーのパラメータ
    public int hp = 1; //敵のHP

//    public bool isRefrect; //反射するか
    public int exp = 1;
    int enemyID;
    protected int currentHP;
    protected Transform hpBar;
    protected bool startFinish;
    private bool isPassable;
    private CircleCollider2D _collider2D;

    //アニメーションが無いため仮　それっぽく魅せる演出用の変数
    float addScale = 3.0f;
    float alpha = 1.0f;

    void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
        isPassable = _collider2D.isTrigger;
    }

    // Use this for initialization
    protected void Start()
    {
        //マネージャーにスポーンしたことを伝える
        EnemyManager.GetInstance.TellSpawn();
        currentHP = hp;
        hpBar = transform.GetChild(1);
        if (hp == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }

        startFinish = true;
    }


    public abstract bool HitByPlayer(Vector2 speed);

    public void TriggerPassableWhenNecessary()
    {
        if (isPassable) return;
        _collider2D.isTrigger = !_collider2D.isTrigger;
    }
}