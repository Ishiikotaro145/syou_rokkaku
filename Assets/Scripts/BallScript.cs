using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public static BallScript instance;
    public GameObject Slash;
    public GameObject ShockWave;
    private const float BallSize = .28f;
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

    private bool gameStart;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        instance = this;
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void GameStart()
    {
        _rigidbody2D.velocity = Vector2.up * 4;
        gameStart = true;
    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Enemy"))
        {
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer()) Instantiate(Slash, o.transform.position, Quaternion.identity);

            StopCoroutine("SlowDown");
            StartCoroutine("SlowDown", 0.4f);
            _rigidbody2D.velocity = _rigidbody2D.velocity + _rigidbody2D.velocity.normalized * .1f;
        }
        else if (o.CompareTag("StoveMouth"))
        {
            UIScript.instance.LifeLoss();
        }

//        else if (o.CompareTag("GameStart"))
//        {
//            StoveScript.instance.GameStart();
//        }
    }

    void OnCollisionEnter2D(Collision2D o)
    {
        if (o.collider.CompareTag("Enemy"))
        {
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer())
                Instantiate(Slash, o.transform.position, Quaternion.identity);

            StopCoroutine("SlowDown");
            StartCoroutine("SlowDown", 0.2f);
            _rigidbody2D.velocity = _rigidbody2D.velocity + _rigidbody2D.velocity.normalized * .1f;
        }
    }

    void LateUpdate()
    {
        if (!gameStart) return;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, _rigidbody2D.velocity);

        // Guide Line

        GuideLines.instance.RemoveAll();
        float remainLength = 5;
        Vector2 lineSpeed = _rigidbody2D.velocity;
        Vector2 linePosition = transform.position;
        RaycastHit2D[] lineHits = Physics2D.CircleCastAll(linePosition, BallSize, lineSpeed, remainLength, 1 << 9);
 
        while (lineHits != null && lineHits.Length > 0)
        {
//            Debug.Log(remainLength);
            int count = 0;
            foreach (var lineHit in lineHits)
            {
                if (lineHit.collider.isTrigger) continue;
                LineRenderer lineRenderer = GuideLines.instance.GetAvailableObject().GetComponent<LineRenderer>();
                if (remainLength < lineHit.distance)
                { 
                    lineRenderer.SetPositions(new[]
                        {(Vector3) linePosition, (Vector3) (linePosition + lineSpeed.normalized * remainLength)});
                    lineRenderer.material.mainTextureScale = new Vector2(remainLength * 5.2f, 1);
                    lineRenderer.gameObject.SetActive(true);
                    return;
                }
                remainLength -= lineHit.distance;
                Vector2 newLinePosition = lineHit.point + lineHit.normal * BallSize;
                
                lineRenderer.SetPositions(new[] {(Vector3) linePosition, (Vector3) newLinePosition});
                lineRenderer.material.mainTextureScale = new Vector2(lineHit.distance * 5.2f, 1);
                lineRenderer.gameObject.SetActive(true);
                linePosition = newLinePosition;
                lineSpeed = Vector2.Reflect(lineSpeed, lineHit.normal);
                lineHits = Physics2D.CircleCastAll(linePosition, BallSize, lineSpeed, remainLength, 1 << 9);
                count = 1;
                break;
            }
            if (count == 0) break;
        }

        LineRenderer lineRendererEnd = GuideLines.instance.GetAvailableObject().GetComponent<LineRenderer>();
        lineRendererEnd.SetPositions(new[]
            {(Vector3) linePosition, (Vector3) (linePosition + lineSpeed.normalized * remainLength)});
        lineRendererEnd.material.mainTextureScale = new Vector2(remainLength * 5.2f, 1);
        lineRendererEnd.gameObject.SetActive(true);
    }

    IEnumerator SlowDown(float scale)
    {
        Time.timeScale = scale;
        yield return new WaitForSeconds(.1f);
        Time.timeScale = 1f;
    }

    public void CreateShokeWave()
    {
        Instantiate(ShockWave, transform.position, transform.rotation).GetComponent<ShockWaveScript>()
            .SetSpeed(2 * _rigidbody2D.velocity);
    }

    public void SetPassable(bool passable)
    {
        if (passable) _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        else _spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}