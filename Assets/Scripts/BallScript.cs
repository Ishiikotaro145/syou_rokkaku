using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    public static BallScript instance;
    private const float BallSize = .15f;
    private Vector2 speed;

    private bool gameStart;
//    private GameObject old;

    private void Start()
    {
        instance = this;
        Reset();
    }

    private void Update()
    {
        if (gameStart == false) return;
        float remainTime = Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, BallSize, speed,
            speed.magnitude * remainTime, ~(1 << 8));
        int count = 0;
        while (hit.collider != null && count < 3)
        {
            GameObject o = hit.collider.gameObject;
            if (o.CompareTag("Stove"))
            {
                remainTime -= hit.distance / speed.magnitude;
                transform.position = hit.point + hit.normal * BallSize;
                speed = Vector2.Reflect(speed, hit.normal);
                speed = speed + 0.07f * speed.normalized;
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
    }

    public void Reset()
    {
        speed = Vector2.up * 2;
        gameStart = false;
        transform.position = new Vector2(0, -2.1f);
    }

    public void GameStart()
    {
        gameStart = true;
    }
}