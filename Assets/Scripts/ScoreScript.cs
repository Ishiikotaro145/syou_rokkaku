using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance;

    private Text text;
    private Animator animator;

    private int score;

    // Use this for initialization
    void Start()
    {
        instance = this;
        text = GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    public void ScorePlusOne()
    {
        text.text = (++score).ToString();
    }

    public void ScoreAnimation()
    {
        animator.SetTrigger("BallOut");
    }
}