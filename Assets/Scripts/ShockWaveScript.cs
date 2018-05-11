using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    // Use this for initialization
    void Start()
    {
//        StartCoroutine("Damage");
        StartCoroutine("Remove");
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, _rigidbody2D.velocity);
    }

    public void SetSpeed(Vector3 speed)
    {
        GetComponent<Rigidbody2D>().velocity = speed;
    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Enemy"))
        {
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            enemyBase.HitByPlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D o)
    {
        if (o.collider.CompareTag("Enemy"))
        {
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            enemyBase.HitByPlayer();
        }
    }
//    IEnumerator Damage()
//    {
//        while (true)
//        {
//            Collider2D hit = Physics2D.OverlapCircle(transform.position, .4f);
//            if (hit != null && hit.CompareTag("Enemy"))
//            {
//                EnemyBase enemyBase = hit.gameObject.GetComponent<EnemyBase>();
//                enemyBase.HitByPlayer();
//            }
//
//            yield return new WaitForSeconds(.1f);
//        }
//    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}