
using System.Linq.Expressions;
using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    public static BallScript instance;
    public AudioClip JumpA;
    public AudioClip ThroughA;
    public AudioClip ReflectA;
    
    public GameObject Slash;

    // public GameObject ShockWave;
    public GameObject LaserPrefab;
    public float maxSpeed = 6;
    private float currentSpeed;
    private const float BallSize = .25f;
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

    private bool gameStart;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    private float lastTimeHitEnemy;

    private void Start()
    {
        instance = this;
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void GameStart(float speed)
    {
        currentSpeed = speed;
        _rigidbody2D.velocity = Vector2.up * currentSpeed;
        lastTimeHitEnemy = Time.time;
        gameStart = true;
    }
public void GameStop(){
    gameStart=false;
    _rigidbody2D.velocity =Vector3.zero;
    GuideLines.instance.RemoveAll();
}
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Enemy"))
        {
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            _audioSource.PlayOneShot(ThroughA);
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer(_rigidbody2D.velocity))
                Instantiate(Slash, o.transform.position, Quaternion.identity);
            lastTimeHitEnemy = Time.time;
            GuideLines.instance.RemoveAll();
            // StopCoroutine("SlowDown");
            // StartCoroutine("SlowDown", 0.4f);
            // currentSpeed += .06f;
            // if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
            // _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * currentSpeed;
        }
        else if (o.CompareTag("StoveMouth"))
        {
            Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            GameScript.instance.LifeLoss();
            StopCoroutine("SlowDown");
            Time.timeScale=1;
            Destroy(gameObject);
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
            _audioSource.PlayOneShot(ReflectA);
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer(_rigidbody2D.velocity))
                Instantiate(Slash, o.transform.position, Quaternion.identity);
            lastTimeHitEnemy = Time.time;
            GuideLines.instance.RemoveAll();
            // StopCoroutine("SlowDown");
            // StartCoroutine("SlowDown", 0.2f);
            // currentSpeed += .09f;
            // if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
            // _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * currentSpeed;
        }
        else if (o.collider.CompareTag("Stove"))
        {
            o.gameObject.GetComponent<WallScript>().Hit(currentSpeed);
            _audioSource.PlayOneShot(JumpA);
            currentSpeed += .09f;
            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * currentSpeed;
        }
    }

    void LateUpdate()
    {
        if (!gameStart) return;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, _rigidbody2D.velocity);

        // Guide Line
        if (Time.time - lastTimeHitEnemy < 5) return;
        GuideLines.instance.RemoveAll();
        float remainLength = 2.5f;
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
        // Time.timeScale = scale;
        // yield return new WaitForSeconds(.1f);
        // Time.timeScale = 1f;

        // yield return new WaitForSecondsRealtime(.2f);
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(.0f);
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(.0f);
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(.0f);
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(.0f);
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(.0f);
        Time.timeScale = scale;
        yield return new WaitForSeconds(.0f);
        Time.timeScale = 1f;
    }
public void StopAllCoroutine(){
    StopCoroutine("SlowDown");
    Time.timeScale=1;
}
    // public void CreateShokeWave()
    // {
    //     Instantiate(ShockWave, transform.position, transform.rotation).GetComponent<ShockWaveScript>()
    //         .SetSpeed(2 * _rigidbody2D.velocity);
    // }

//    public void SetPassable(bool passable)
//    {
//        if (passable) _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
//        else _spriteRenderer.color = new Color(1, 1, 1, 1);
//    }
}