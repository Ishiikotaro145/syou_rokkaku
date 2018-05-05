using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{
    private Vector3 speed;
    private const float BallSize = .3f;

    private bool move;

    // Use this for initialization
    void Start()
    {
//        StartCoroutine("Damage");
        StartCoroutine("Remove");
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)return;
        float remainTime = Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, BallSize, speed,
            speed.magnitude * remainTime, ~(1 << 8));
        int count = 0;
        while (hit.collider != null && count < 3)
        {
            GameObject o = hit.collider.gameObject;
//            Debug.Log(o.name);
            if (o.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>(); 
                enemyBase.HitByPlayer();

                if (enemyBase.isRefrect)
                {
                    remainTime -= hit.distance / speed.magnitude;
                    transform.position = hit.point + hit.normal * BallSize;
                    speed = Vector2.Reflect(speed, hit.normal); 
                } 
            }
            else if (o.CompareTag("Stove"))
            {
                remainTime -= hit.distance / speed.magnitude;
                transform.position = hit.point + hit.normal * BallSize;
                speed = Vector2.Reflect(speed, hit.normal);
//                speed = speed + 0.07f * speed.normalized; 
            } 
            else
            {
                break;
            }

            count++;
            hit = Physics2D.CircleCast(transform.position, BallSize, speed, speed.magnitude * remainTime, ~(1 << 8));
        }

        transform.position += (Vector3) speed * remainTime;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, speed);
    }

    public void SetSpeed(Vector3 speed)
    {
        this.speed = speed;
        move = true;
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