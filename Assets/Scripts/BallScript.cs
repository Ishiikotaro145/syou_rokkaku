using System.Collections;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject Slash;
    public static BallScript instance;
    private const float BallSize = .23f;
    private Vector2 speed;

    private Animator _animator;
//    private Rigidbody2D _rigidbody2D;

    private bool gameStart;
//    private GameObject old;

    private void Start()
    {
        instance = this;
        Reset();
        _animator = GetComponent<Animator>();
//        _rigidbody2D = GetComponent<Rigidbody2D>();
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

            count++;
            hit = Physics2D.CircleCast(transform.position, BallSize, speed, speed.magnitude * remainTime, ~(1 << 8));
        }

        transform.position += (Vector3) speed * remainTime;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, speed);

        // Guide Line

        GuideLines.instance.RemoveAll();
        float remainLength = 20;
        Vector2 lineSpeed = speed;
        Vector2 linePosition = transform.position;
        RaycastHit2D lineHit = Physics2D.CircleCast(linePosition, BallSize, lineSpeed, remainLength, 1 << 9);

        while (lineHit.collider != null)
        {
            Debug.Log(remainLength);
            if (remainLength < lineHit.distance) break;
            remainLength -= lineHit.distance;
            Vector2 newLinePosition = lineHit.point + lineHit.normal * BallSize;
            LineRenderer lineRenderer = GuideLines.instance.GetAvailableObject().GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new[] {(Vector3) linePosition, (Vector3) newLinePosition});
            lineRenderer.gameObject.SetActive(true);
            linePosition = newLinePosition;
            lineSpeed = Vector2.Reflect(lineSpeed, lineHit.normal);
            lineHit = Physics2D.CircleCast(linePosition, BallSize, lineSpeed, remainLength, 1 << 9);
        }

        LineRenderer lineRendererEnd = GuideLines.instance.GetAvailableObject().GetComponent<LineRenderer>();
        lineRendererEnd.SetPositions(new[]
            {(Vector3) linePosition, (Vector3) (linePosition + lineSpeed.normalized * remainLength)});
        lineRendererEnd.gameObject.SetActive(true);
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
//        _rigidbody2D.velocity = 4*Vector2.up;
    }

    IEnumerator SlowDown(float[] scaleAndDuration)
    {
        Time.timeScale = scaleAndDuration[0];
        yield return new WaitForSecondsRealtime(scaleAndDuration[1]);
        Time.timeScale = 1f;
    }
}