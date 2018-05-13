using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;
    public GameObject hearts;
    public GameObject chargeBar;
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
    private bool gameStart;
    private bool gameOver;
    private bool gameReset = true;
    private BallScript playerInstance;

    private float chargeStartTime;
//    private Image chargeBarImage;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        speed = startYusyaSpeed;
        heartArray = new[]
        {
            hearts.transform.GetChild(0).gameObject, hearts.transform.GetChild(1).gameObject,
            hearts.transform.GetChild(2).gameObject
        };
        playerInstance = Instantiate(player, new Vector2(0, startYusyaPositionY), Quaternion.FromToRotation(Vector3.right, Vector3.up))
            .GetComponent<BallScript>();
//        playerInstance.transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.up);
//        chargeBarImage = chargeBar.GetComponent<Image>();
    }

    public void LifeLoss()
    {
        if (life == 0) return;
        life--;
        heartArray[life].active = false;
        gameStart = false;
        gameReset = true;
        chargeBar.transform.localScale = new Vector2(0, 1);
        if (life == 0)
        {
            gameOverUI.active = true;
            gameOver = true;
            StartCoroutine("NextScene");
        }
        else
        {
            Destroy(playerInstance.gameObject);
            speed = startYusyaSpeed;
            StageManager.GetInstance.RestorePassableImmediately();
            GuideLines.instance.RemoveAll();
            StoveScript.instance.Reset();
            tapToStart.active = true;
            playerInstance =
                Instantiate(player, new Vector2(0, startYusyaPositionY), Quaternion.FromToRotation(Vector3.right, Vector3.up))
                    .GetComponent<BallScript>();
        }
    }

    public void WaveClear()
    {
        gameStart = false;
        gameReset = true;
        speed = playerInstance.GetCurrentSpeed();
        Destroy(playerInstance.gameObject);
        
//        StageManager.GetInstance.RestorePassableImmediately();
        GuideLines.instance.RemoveAll();
//        StoveScript.instance.Reset();
        tapToStart.active = true;
        playerInstance =
            Instantiate(player, new Vector2(0, startYusyaPositionY), Quaternion.FromToRotation(Vector3.right, Vector3.up))
                .GetComponent<BallScript>();
    }
    
    private void Update()
    {
        if (gameReset && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            gameReset = false;
        }
        else if (!gameStart && !gameOver && !gameReset && Input.GetMouseButtonUp(0))
        {
            tapToStart.active = false;
            StartCoroutine("GameStart");

            gameStart = true;
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

    public void GameClear()
    {
        clearUI.active = true;
        StartCoroutine("NextScene");
    }

//    public void ScorePlusOne()
//    {
//        text.text = (++score).ToString();
//    }
//
//    public void ScoreAnimation()
//    {
//        animator.SetTrigger("BallOut");
//    }
    private IEnumerator NextScene()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StageSelect");
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(.5f);
        StageManager.GetInstance.GameStart();
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