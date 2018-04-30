using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    private const float BallSize = .15f;
    private Vector2 speed = Vector2.up * 2;
    private bool gameStart;
    private GameObject old;

    private void Update()
    {
        float remainTime = Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, BallSize, speed,
            speed.magnitude * remainTime);
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
                if (!o.Equals(old))
                {
                    ScoreScript.instance.ScorePlusOne();
                    old = o;
                }
            }
            else if (o.CompareTag("StoveMouth"))
            {
                if (gameStart)
                {
                    ScoreScript.instance.ScoreAnimation();
                    StartCoroutine("NextScene");
                }
                break;
            }
            else if (o.CompareTag("GameStart"))
            {
                gameStart = true;
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
    
    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stove");
    }
}