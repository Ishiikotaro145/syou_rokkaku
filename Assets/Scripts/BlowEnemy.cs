using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowEnemy : EnemyBase
{
    private Rigidbody2D _rigidbody2D;
    public GameObject ParticlePrefab;
    public GameObject LaserPrefab;

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
            o.gameObject.GetComponent<EnemyBase>().HitByPlayer(_rigidbody2D.velocity);
        }
        else if (o.CompareTag("StoveMouth"))
        {
            Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); 
        }
    }

    void OnCollisionEnter2D(Collision2D o)
    {
        if (o.collider.CompareTag("Enemy"))
        {
            o.gameObject.GetComponent<EnemyBase>().HitByPlayer(_rigidbody2D.velocity);
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
            _rigidbody2D.velocity = -2 * speed;
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
        if (_rigidbody2D.velocity.magnitude < 0.3)
        {
            Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); 
        }
        Damage();
    }
}