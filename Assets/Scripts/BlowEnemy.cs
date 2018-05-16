using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowEnemy : EnemyBase
{
    private Rigidbody2D _rigidbody2D;
    public GameObject ParticlePrefab;
    public GameObject HitOthersPrefab;
    public GameObject LaserPrefab;
    public AudioClip dieIn;
    public AudioClip dieOut;
    public AudioClip hit;
    public AudioClip reflect;

    public int hitWallTimes = 1; 

    private bool isDead;

    new void Start()
    {
        base.Start();
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Enemy"))
        {
            AudioScript.instance.PlayOneShot(hit);
//            Debug.Log("HitOthers");
            o.gameObject.GetComponent<EnemyBase>().HitByPlayer(_rigidbody2D.velocity);
            Instantiate(HitOthersPrefab, transform.position, Quaternion.identity);
        }
        else if (o.CompareTag("StoveMouth"))
        {
            Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            AudioScript.instance.PlayOneShot(dieOut);
            EnemyManager.GetInstance.TellDead(); 
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D o)
    {
        if (o.collider.CompareTag("Enemy"))
        {
//            Debug.Log("HitOthers");
            AudioScript.instance.PlayOneShot(hit);
            o.gameObject.GetComponent<EnemyBase>().HitByPlayer(_rigidbody2D.velocity);
            Instantiate(HitOthersPrefab, transform.position, Quaternion.identity);
        }
        else if (o.collider.CompareTag("Stove"))
        {

            hitWallTimes--;
            if (hitWallTimes == 0)
            {
                Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
                Debug.Log("dieIn");
                AudioScript.instance.PlayOneShot(dieIn,5);
                EnemyManager.GetInstance.TellDead(); 
                Destroy(gameObject);
            }
            else{
                AudioScript.instance.PlayOneShot(reflect);
            }
        }
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
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.velocity = -1.6f * speed; //吹き飛ばし速度
            gameObject.layer = 11;
            isDead = true;

            //EnemyManager.GetInstance.TellDead();
        }

        isDamage = true;
        damageTime = 0;


        return true;
    }

    void Update()
    {
        if (!isDead) return;
        transform.Rotate(0, 0, 2160 * Time.deltaTime);

        Damage();
    }
}