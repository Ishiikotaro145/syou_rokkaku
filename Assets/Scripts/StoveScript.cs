using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    public static StoveScript instance;
    public GameObject GameStartTrigger;
    public GameObject HPBar;
    public int HpPerWall;
    public float LeastSpeedToBreak;

    private Vector2 mousePositionOld;

    private bool gameStart;

    private bool gameReset = true;

    private int totalWallCnt = 6;
    private int currentWallCnt = 6;
    private int currentHP;
    private Image HPBarImage;


    // Use this for initialization
    void Awake()
    {
        instance = this;
        currentHP = (totalWallCnt - 1) * HpPerWall;
        HPBarImage = HPBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart) return;
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            if (Vector2.Distance(mousePositionOld, Vector2.zero) > .5f)
            gameReset = false;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            if (Vector2.Distance(mousePositionOld, Vector2.zero) > .5f)
            {
                if (!gameReset)
                {
                    Quaternion rotation = Quaternion.FromToRotation(mousePositionOld, mousePosition);
                    gameObject.transform.rotation = gameObject.transform.rotation * rotation;
                }
                else gameReset = false;
            }

            mousePositionOld = mousePosition;
        }
    }

    public bool HitStoveWall(float currentSpeed)
    {
        if (currentSpeed < LeastSpeedToBreak || currentHP <= 0) return false;
        currentHP--;
        HPBarImage.fillAmount = (float) currentHP / (HpPerWall * (totalWallCnt - 1));
        if (currentHP < (currentWallCnt - 1) * HpPerWall)
        {
            currentWallCnt--;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameStart = false;
        gameReset = true;
        GameStartTrigger.SetActive(true);
    }

    public void GameStart()
    {
        gameStart = true;
        GameStartTrigger.SetActive(false);
    }
}