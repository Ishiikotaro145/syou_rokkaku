using System.Collections;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject Slash;
    public GameObject ShockWave;
    private const float BallSize = .28f;
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

    private bool gameStart;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = Vector2.up * 4;
    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Enemy"))
        {
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer()) Instantiate(Slash, o.transform.position, Quaternion.identity);

            StopCoroutine("SlowDown");
            StartCoroutine("SlowDown", new[] {0.4f, 0.2f});
        }
        else if (o.CompareTag("StoveMouth"))
        {
            UIScript.instance.LifeLoss();
        }
        else if (o.CompareTag("GameStart"))
        {
            StoveScript.instance.GameStart();
        }
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
            StartCoroutine("SlowDown", new[] {0.2f, 0.4f});
        }
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, _rigidbody2D.velocity);

        // Guide Line

        GuideLines.instance.RemoveAll();
        float remainLength = 5;
        Vector2 lineSpeed = _rigidbody2D.velocity;
        Vector2 linePosition = transform.position;
        RaycastHit2D lineHit = Physics2D.CircleCast(linePosition, BallSize, lineSpeed, remainLength, 1 << 9);

        while (lineHit.collider != null)
        {
//            Debug.Log(remainLength);
            if (remainLength < lineHit.distance) break;
            remainLength -= lineHit.distance;
            Vector2 newLinePosition = lineHit.point + lineHit.normal * BallSize;
            LineRenderer lineRenderer = GuideLines.instance.GetAvailableObject().GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new[] {(Vector3) linePosition, (Vector3) newLinePosition});
            lineRenderer.material.mainTextureScale = new Vector2(lineHit.distance * 5.2f, 1);
            lineRenderer.gameObject.SetActive(true);
            linePosition = newLinePosition;
            lineSpeed = Vector2.Reflect(lineSpeed, lineHit.normal);
            lineHit = Physics2D.CircleCast(linePosition, BallSize, lineSpeed, remainLength, 1 << 9);
        }

        LineRenderer lineRendererEnd = GuideLines.instance.GetAvailableObject().GetComponent<LineRenderer>();
        lineRendererEnd.SetPositions(new[]
            {(Vector3) linePosition, (Vector3) (linePosition + lineSpeed.normalized * remainLength)});
        lineRendererEnd.material.mainTextureScale = new Vector2(remainLength * 5.2f, 1);
        lineRendererEnd.gameObject.SetActive(true);
    }


    IEnumerator SlowDown(float[] scaleAndDuration)
    {
        Time.timeScale = scaleAndDuration[0];
        yield return new WaitForSecondsRealtime(scaleAndDuration[1]);
        Time.timeScale = 1f;
    }

    public void CreateShokeWave()
    {
        Instantiate(ShockWave, transform.position, transform.rotation).GetComponent<ShockWaveScript>()
            .SetSpeed(2 * _rigidbody2D.velocity);
    }
}