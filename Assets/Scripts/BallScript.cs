using System.Collections;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject Slash;
    public static BallScript instance;
    private const float BallSize = .25f;
    private Vector2 speed;
    private Animator _animator;

    private bool gameStart;
//    private GameObject old;

    private void Start()
    {
        instance = this;
        Reset();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameStart == false) return;
        float remainTime = Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, BallSize, speed,
            speed.magnitude * remainTime, ~(1 << 5));
        int count = 0;
        while (hit.collider != null && count < 3)
        {
            GameObject o = hit.collider.gameObject;
//            Debug.Log(o.name);
            if (o.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
                _animator.SetTrigger("Attack");
                Instantiate(Slash, hit.point + hit.normal * BallSize, Quaternion.identity);
                enemyBase.HitByPlayer();

                if (enemyBase.isRefrect)
                {
                    remainTime -= hit.distance / speed.magnitude;
                    transform.position = hit.point + hit.normal * BallSize;
                    speed = Vector2.Reflect(speed, hit.normal);
                    StopCoroutine("SlowDown");
                    StartCoroutine("SlowDown", new[] {0.2f, 0.4f});
                }
                else
                {
                    StopCoroutine("SlowDown");
                    StartCoroutine("SlowDown", new[] {0.4f, 0.2f});
                }
            }
            else if (o.CompareTag("Stove"))
            {
                remainTime -= hit.distance / speed.magnitude;
                transform.position = hit.point + hit.normal * BallSize;
                speed = Vector2.Reflect(speed, hit.normal);
//                speed = speed + 0.07f * speed.normalized;
//                if (!o.Equals(old))
//                {
//                    UIScript.instance.ScorePlusOne();
//                    old = o;
//                }
            }
            else if (o.CompareTag("StoveMouth"))
            {
                UIScript.instance.LifeLoss();
                break;
            }
            else
            {
                break;
            }

            hit = Physics2D.CircleCast(transform.position, BallSize, speed, speed.magnitude * remainTime);
            count++;
        }

        transform.position += (Vector3) speed * remainTime;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, speed);
    }

    public void Reset()
    {
        speed = Vector2.up * 4;
        gameStart = false;
        transform.position = new Vector2(0, -4.5f);
    }

    public void GameStart()
    {
        gameStart = true;
    }

    IEnumerator SlowDown(float[] scaleAndDuration)
    {
        Time.timeScale = scaleAndDuration[0];
        yield return new WaitForSecondsRealtime(scaleAndDuration[1]);
        Time.timeScale = 1f;
    }
}