using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;
    public GameObject hearts;
    public GameObject chargeBar;
    public GameObject tapToStart;
    public GameObject gameOverUI;
    public GameObject clearUI;
    public GameObject player;

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
        heartArray = new[]
        {
            hearts.transform.GetChild(0).gameObject, hearts.transform.GetChild(1).gameObject,
            hearts.transform.GetChild(2).gameObject
        };
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
            Destroy(playerInstance);
            GuideLines.instance.RemoveAll();
            StoveScript.instance.Reset();
            tapToStart.active = true;
        }
    }

    private void Update()
    {
        if (gameReset && Input.GetMouseButtonDown(0))
        {
            gameReset = false;
        }
        else if (!gameStart && !gameOver && !gameReset && Input.GetMouseButtonUp(0))
        {
            tapToStart.active = false;
            StartCoroutine("GameStart");

            gameStart = true;
        }

        else if (gameStart && !gameReset)
        {
            if (Input.GetMouseButtonDown(0)) chargeStartTime = Time.time;
            else if (Input.GetMouseButton(0))
                chargeBar.transform.localScale = new Vector2(Mathf.Min((Time.time - chargeStartTime) / 2f, 1), 1);
            else if (Input.GetMouseButtonUp(0))
            {
                chargeBar.transform.localScale = new Vector2(0, 1);
                if (Time.time - chargeStartTime > 2) playerInstance.CreateShokeWave();
            }
        }
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StageSelect");
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(.5f);
        StageManager.GetInstance.GameStart();
//        yield return new WaitForSeconds(.5f);
        playerInstance = Instantiate(player, new Vector2(0, -5f), Quaternion.identity).GetComponent<BallScript>();
    }
}