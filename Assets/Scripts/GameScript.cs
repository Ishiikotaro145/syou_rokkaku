using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    public static GameScript instance;

    public GameObject hearts;
    public GameObject Tutor;
    public AudioClip LossHeartA;
    public AudioClip WaveClearA;
    public AudioClip GameOverA;
    public AudioClip StageClearA;

//    public GameObject chargeBar;
    public GameObject tapToStart;
    public GameObject gameOverUI;
    public GameObject clearUI;
    public GameObject player;
    public GameObject pausePanel;
    public float startYusyaPositionY;
    public float startYusyaSpeed;
    private float speed;

    private GameObject[] heartArray;
    private int score;

    private int life = 3;

//    private bool gameStart;
    private bool gameClear;
    private bool readyToStart;
    private bool isMouseDown;
    private BallScript playerInstance;

    private float chargeStartTime;

    private AudioSource _audioSource;
//    private Image chargeBarImage;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        speed = startYusyaSpeed;
        heartArray = new[]
        {
            hearts.transform.GetChild(0).gameObject, hearts.transform.GetChild(1).gameObject,
            hearts.transform.GetChild(2).gameObject
        };
        playerInstance = Instantiate(player, new Vector2(0, startYusyaPositionY),
                Quaternion.FromToRotation(Vector3.right, Vector3.up))
            .GetComponent<BallScript>();

        StageManager.GetInstance.GameStart();
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1)
        {
            StartCoroutine("OpenTutor");
        }
        else PrepareForNextTurn();

//        playerInstance.transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.up);
//        chargeBarImage = chargeBar.GetComponent<Image>();
    }

    public void LifeLoss()
    {
        if (life == 0 || gameClear) return;
        life--;
        _audioSource.PlayOneShot(LossHeartA);
        heartArray[life].SetActive(false);
//        chargeBar.transform.localScale = new Vector2(0, 1);
        if (life == 0)
        {
            gameOverUI.SetActive(true);
            _audioSource.PlayOneShot(GameOverA);
            GuideLines.instance.RemoveAll();
            StartCoroutine("NextScene");
        }
        else
        {
            speed = startYusyaSpeed;
//            StageManager.GetInstance.RestorePassableImmediately();
            GuideLines.instance.RemoveAll();
            StoveScript.instance.Reset();
            playerInstance =
                Instantiate(player, new Vector2(0, startYusyaPositionY),
                        Quaternion.FromToRotation(Vector3.right, Vector3.up))
                    .GetComponent<BallScript>();
            PrepareForNextTurn();
        }
    }

    public void WaveClear()
    {
//        StageManager.GetInstance.SetWaveClear(); 
        _audioSource.PlayOneShot(WaveClearA);
        speed = playerInstance.GetCurrentSpeed();
        Destroy(playerInstance.gameObject);

//        StageManager.GetInstance.RestorePassableImmediately();
        GuideLines.instance.RemoveAll();
//        StoveScript.instance.Reset();
        playerInstance =
            Instantiate(player, new Vector2(0, startYusyaPositionY),
                    Quaternion.FromToRotation(Vector3.right, Vector3.up))
                .GetComponent<BallScript>();
    }

    private void Update()
    {
        if (Tutor.active && Input.GetMouseButtonUp(0))
        {
            StartCoroutine("CloseTutor");
        }

        if (!isMouseDown && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            isMouseDown = true;
        }
        else if (readyToStart && isMouseDown && Input.GetMouseButtonUp(0))
        {
            tapToStart.SetActive(false);
            StartCoroutine("GameStart");
            readyToStart = false;
        }

//        else if (gameStart && !gameReset)
//        {
//            if (Input.GetMouseButtonDown(0)) chargeStartTime = Time.unscaledTime;
//            else if (Input.GetMouseButton(0))
//                chargeBar.transform.localScale = new Vector2(Mathf.Min((Time.unscaledTime - chargeStartTime) / 3f, 1), 1);
//            else if (Input.GetMouseButtonUp(0))
//            {
//                chargeBar.transform.localScale = new Vector2(0, 1);
//                if (Time.unscaledTime - chargeStartTime > 3)
//                {
//                    StageManager.GetInstance.TriggerEnemyPassable();
//                    playerInstance.SetPassable(true);
//                }
//            }
//        }
    }

    IEnumerator OpenTutor()
    { 
        yield return new WaitForSeconds(.5f);
        Tutor.SetActive(true);
    }
    
    IEnumerator CloseTutor()
    {
        Tutor.SetActive(false);
        yield return new WaitForSeconds(.5f);
        PrepareForNextTurn();
    }
    public void PrepareForNextTurn()
    {
        tapToStart.SetActive(true);
        readyToStart = true;
        isMouseDown = false;
    }

    public void GameClear()
    {
        _audioSource.PlayOneShot(StageClearA);
        clearUI.active = true;
        gameClear = true;
//        StartCoroutine("NextScene");
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSecondsRealtime(3f);
        GoToMenu();
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(.5f);
//        yield return new WaitForSeconds(.5f);
//        playerInstance = Instantiate(player, new Vector2(0, -2f), Quaternion.identity).GetComponent<BallScript>();
        playerInstance.GameStart(speed);
        StoveScript.instance.GameStart();
    }

    public void Pause(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StageSelect");
    }
}