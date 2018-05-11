using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public static BallScript instance;
    public GameObject Slash;
    public GameObject ShockWave;
    public AudioClip HitWall;
      public AudioClip ReflectEnmy;
        public AudioClip ThroughEnmy;
    private const float BallSize = .28f;
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

    private bool gameStart;
    private SpriteRenderer _spriteRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        instance = this;
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource=GetComponent<AudioSource>();
    }

    public void GameStart()
    {
        _rigidbody2D.velocity = Vector2.up * 3;
        gameStart = true;
    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(ThroughEnmy,1);
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer()) Instantiate(Slash, o.transform.position, Quaternion.identity);

            // StopCoroutine("SlowDown");
            // StartCoroutine("SlowDown", 0.2f);
            _rigidbody2D.velocity = _rigidbody2D.velocity + _rigidbody2D.velocity.normalized * .03f;//貫通敵の反射加速度
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
            audioSource.PlayOneShot(ReflectEnmy,1);
            EnemyBase enemyBase = o.gameObject.GetComponent<EnemyBase>();
            _animator.SetTrigger("Attack");

            if (enemyBase.HitByPlayer())
                Instantiate(Slash, o.transform.position, Quaternion.identity);

            StopCoroutine("SlowDown");
            StartCoroutine("SlowDown", 0.1f);
            _rigidbody2D.velocity = _rigidbody2D.velocity + _rigidbody2D.velocity.normalized * .06f;//反射敵の反射加速度
        }
        else if (o.collider.CompareTag("Stove"))
        {
            if (StoveScript.instance.HitStoveWall(_rigidbody2D.velocity.magnitude))
                o.gameObject.SetActive(false);
            audioSource.PlayOneShot(HitWall,1);
        }
    }

    void LateUpdate()
    {
        if (!gameStart) return;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, _rigidbody2D.velocity);

        // Guide Line

        GuideLines.instance.RemoveAll();
        float remainLength = 3.5f; //nagasa
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
        // Debug.Log("Slow");
        // yield return new WaitForSecondsRealtime(.2f);
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(.005f);
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(.01f);
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(.016f);
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(.01f);
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(.05f);
        Time.timeScale = scale;
        yield return new WaitForSeconds(.0f);
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