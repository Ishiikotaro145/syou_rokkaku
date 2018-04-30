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
    public GameObject tapToStart;
    public GameObject gameOver;

    private GameObject[] heartArray;
    private int score;
    private int life = 3;
    private bool gameStart;

    // Use this for initialization
    void Start()
    {
        instance = this;
        heartArray = new[]
        {
            hearts.transform.GetChild(0).gameObject, hearts.transform.GetChild(1).gameObject,
            hearts.transform.GetChild(2).gameObject
        };
    }

    public void LifeLoss()
    {
        if (life == 0) return;
        life--;
        heartArray[life].active = false;
        gameStart = false;
        if (life == 0)
        {
            gameOver.active = true;
            StartCoroutine("NextScene");
        }
        else
        {
            BallScript.instance.Reset();
            StoveScript.instance.Reset();
            tapToStart.active = true;
        }
    }

    private void Update()
    {
        if (!gameStart && Input.GetMouseButtonUp(0))
        {
            tapToStart.active = false;
            StartCoroutine("GameStart");
            
            gameStart = true;
        }
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
        yield return new WaitForSeconds(.5f); 
        BallScript.instance.GameStart();
        StoveScript.instance.GameStart();
    }
}