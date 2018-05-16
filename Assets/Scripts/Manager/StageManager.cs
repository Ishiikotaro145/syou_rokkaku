using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum STAGE : int
{
    STAGE1_1,
    STAGE1_2,
    STAGE1_3,
    STAGE2_1,
    STAGE2_2,
    STAGE2_3,
    STAGE3_1,
    STAGE3_2,
    STAGE3_3,
};

enum WAVECLEAR : int
{
    WAVE,
    CLEAR,
    BACK_OVERRAY,
    W1,
    W2,
    W3,
    W4,
    W5,
    W6,
    W7,
    W8,
    W9,
}


public class StageManager : SingletonBase<StageManager>
{
    public List<GameObject> pStage1_1;
    public List<GameObject> pStage1_2;
    public List<GameObject> pStage1_3;
    public List<GameObject> pStage2_1;
    public List<GameObject> pStage2_2;
    public List<GameObject> pStage2_3;
    public List<GameObject> pStage3_1;
    public List<GameObject> pStage3_2;
    public List<GameObject> pStage3_3;

    public List<GameObject> pWaveClear;

    List<GameObject> SelectStageList = new List<GameObject>();
    private GameObject allEnemyInStage;

    private bool gameStart;
    int nowStage = -1;
    int nowWave = 0;
    int maxWaveCnt = 0;


//    private bool enemyPassable;

    //WAVE STARTの表示に必要なもの
    private bool isWaveStart = false;
    float  waveStartPos = 0.0f;
    bool isWaveStartBay = false;

    //WAVE CLEARの表示に必要な画像
    const float waveClearDistance = 0.2f;
    private bool isWaveClear = false;
    int waveClearTime = 0;
    float overAlpha = 0.5f;
    float overScale = 2.0f;
    float waveClearPosX = waveClearDistance;
    float waveClearPosXConst = waveClearDistance;


    //WAVEクリア　フラグを取得
    public bool GetisWaveClear()
    {
        return isWaveClear;
    }


    public void SetWaveClear()
    {
        if (allEnemyInStage != null) Destroy(allEnemyInStage);
//        Debug.Log("wave   " + nowWave);
        if (nowWave >= maxWaveCnt)
        {
            GameScript.instance.GameClear();
            return;
        }

        GameScript.instance.WaveClear();
        isWaveClear = true;
    }


    public void SetWaveStart()
    {
        isWaveStart = true;
        waveStartPos = 10.0f;
        isWaveStartBay = false;
    }


    public void SetWaveStartBay()
    {
        isWaveStartBay = true;
        waveStartPos = -0.1f;
    }


    void Awake()
    {
        if (this != GetInstance)
        {
            Destroy(this);
            return;
        }

//        DontDestroyOnLoad(this.gameObject);
    }


    void Update()
    {
        WaveClear();

        WaveStart();
    }


    void WaveClear()
    {
        if (isWaveClear == false) return;
        waveClearTime++;
        //オーバーレイ　白
        if (waveClearTime < 50)
        {
            overAlpha *= 0.95f;
            overScale *= 0.92f;

            waveClearPosX *= 0.95f;
        }

        if (waveClearTime == 80)
        {
            waveClearPosXConst = 10.0f;
            waveClearPosX = waveClearPosXConst;
        }

        if (waveClearTime > 80)
        {
            overAlpha += 0.05f;
            waveClearPosX *= 0.95f;
        }

        SpriteRenderer sprite = pWaveClear[(int) WAVECLEAR.BACK_OVERRAY].GetComponent<SpriteRenderer>();
        var color = sprite.color;
        color.a = 0.5f - overAlpha;
        sprite.color = color;
        pWaveClear[(int) WAVECLEAR.BACK_OVERRAY].transform.localScale = new Vector3(20.0f, 2.0f - overScale, 1.0f);

        //WAVE CLEAR
        SpriteRenderer waveSprite = pWaveClear[(int) WAVECLEAR.WAVE].GetComponent<SpriteRenderer>();
        var waveColor = waveSprite.color;
        waveColor.a = 1.0f - overAlpha;
        waveSprite.color = waveColor;
        pWaveClear[(int) WAVECLEAR.WAVE].transform.position =
            new Vector3(-1.37f - (waveClearPosXConst - waveClearPosX), 0.0f, 0.0f);

        SpriteRenderer clearSprite = pWaveClear[(int) WAVECLEAR.CLEAR].GetComponent<SpriteRenderer>();
        var clearColor = clearSprite.color;
        clearColor.a = 1.0f - overAlpha;
        clearSprite.color = clearColor;
        pWaveClear[(int) WAVECLEAR.CLEAR].transform.position =
            new Vector3(1.54f + (waveClearPosXConst - waveClearPosX), 0.0f, 0.0f);

        SpriteRenderer nowSprite = pWaveClear[nowWave + 3].GetComponent<SpriteRenderer>();
        var nowColor = nowSprite.color;
        nowColor.a = 1.0f - overAlpha;
        nowSprite.color = nowColor;

        pWaveClear[nowWave + 3].transform.position = new Vector3(0.0f, 0.0f, 0.0f);


        if (waveClearTime >= 150)
        {
            isWaveClear = false;
            waveClearTime = 0;
            overAlpha = 0.5f;
            overScale = 2.0f;
            waveClearPosX = waveClearDistance;
            waveClearPosXConst = waveClearDistance;

            GameScript.instance.PrepareForNextTurn();
            NextWave();
        }
    }


        void WaveStart()
    {
        if (isWaveStart == false)return;

        if (isWaveStartBay == false) 
        {
            waveStartPos *= 0.95f;
        } 
        else
        {
            waveStartPos -= 0.05f;
            waveStartPos *= 1.4f;
        }

        //WAVE CLEAR
        SpriteRenderer waveSprite = pWaveClear [(int)WAVECLEAR.WAVE].GetComponent<SpriteRenderer> ();
        var waveColor = waveSprite.color;
        waveColor.a = 1.0f;
        waveSprite.color = waveColor;
        pWaveClear [(int)WAVECLEAR.WAVE].transform.position = new Vector3 (waveStartPos - 0.2f,0.0f,0.0f);


        SpriteRenderer nowSprite = pWaveClear [nowWave + 3].GetComponent<SpriteRenderer> ();
        var nowColor = nowSprite.color;
        nowColor.a = 1.0f;
        nowSprite.color = nowColor;
        pWaveClear [nowWave + 3].transform.position = new Vector3 (waveStartPos + 1.2f,0.0f,0.0f);


        if(waveStartPos < -10.0f)
        {
            isWaveStart = false;
        }
        Debug.Log(waveStartPos);
    }


    public void GameStart()
    {
//        Debug.Log(nowStage + ", " + gameStart);
        if (!gameStart)
        {
            gameStart = true;
            nowStage = PlayerPrefs.GetInt("StageSelect");
            switch (nowStage)
            {
                case (int) STAGE.STAGE1_1:
                    maxWaveCnt = pStage1_1.Count;
                    StageCopy(pStage1_1);
                    break;
                case (int) STAGE.STAGE1_2:
                    maxWaveCnt = pStage1_2.Count;
                    StageCopy(pStage1_2);
                    break;
                case (int) STAGE.STAGE1_3:
                    maxWaveCnt = pStage1_3.Count;
                    StageCopy(pStage1_3);
                    break;
                case (int) STAGE.STAGE2_1:
                    maxWaveCnt = pStage2_1.Count;
                    StageCopy(pStage2_1);
                    break;
                case (int) STAGE.STAGE2_2:
                    maxWaveCnt = pStage2_2.Count;
                    StageCopy(pStage2_2);
                    break;
                case (int) STAGE.STAGE2_3:
                    maxWaveCnt = pStage2_3.Count;
                    StageCopy(pStage2_3);
                    break;
                case (int) STAGE.STAGE3_1:
                    maxWaveCnt = pStage3_1.Count;
                    StageCopy(pStage3_1);
                    break;
                case (int) STAGE.STAGE3_2:
                    maxWaveCnt = pStage3_2.Count;
                    StageCopy(pStage3_2);
                    break;
                case (int) STAGE.STAGE3_3:
                    maxWaveCnt = pStage3_3.Count;
                    StageCopy(pStage3_3);
                    break;
            }


            NextWave();
//            Debug.Log("ステージマネージャーStart");
        }
    }


//    // Update is called once per frame
//    void Update()
//    {
//        if (EnemyManager.GetInstance.GetNowEnemyCnt() <= 0)
//        {
//            if (nowWave >= maxWaveCnt)
//            {
//                //ステージクリア
//            }
//            else
//            {
//                //次のWAVEの敵を出す。
//                GameObject stageObject = (GameObject) Instantiate
//                (
//                    SelectStageList[nowWave],
//                    transform.position,
//                    Quaternion.identity
//                );
//                nowWave++;
//
////                Debug.Log ("Waveエネミー生成完了");
//            }
//        }
//    }

    void NextWave()
    {
        //次のWAVEの敵を出す。
        allEnemyInStage = Instantiate
        (
            SelectStageList[nowWave],
            transform.position,
            Quaternion.identity
        );
//        if (enemyPassable)
//        {
//            EnemyBase[] enemyBases = allEnemyInStage.transform.GetComponentsInChildren<EnemyBase>();
//            foreach (var enemy in enemyBases)
//            {
//                enemy.TriggerPassableWhenNecessary();
//            }
//        }
        //Debug.Log ("WAVEStart!!!!!!!!");
        nowWave++;
        SetWaveStart ();
//              Debug.Log ("Waveエネミー生成完了");
    }

//    public void TriggerEnemyPassable()
//    {
//        if (enemyPassable)
//        {
//            StopCoroutine("RestorePassable");
//            StartCoroutine("RestorePassable");
//            return;
//        }
//
//        enemyPassable = true;
//        if (allEnemyInStage != null)
//        {
//            EnemyBase[] enemyBases = allEnemyInStage.transform.GetComponentsInChildren<EnemyBase>();
//            foreach (var enemy in enemyBases)
//            {
//                enemy.TriggerPassableWhenNecessary();
//            }
//        }
//
//        StartCoroutine("RestorePassable");
//    }
//
//    IEnumerator RestorePassable()
//    {
//        yield return new WaitForSeconds(2);
//        RestorePassableImmediately();
//    }
//
//    public void RestorePassableImmediately()
//    {
//        if (!enemyPassable) return;
//        if (allEnemyInStage != null)
//        {
//            EnemyBase[] enemyBases = allEnemyInStage.transform.GetComponentsInChildren<EnemyBase>();
//            foreach (var enemy in enemyBases)
//            {
//                enemy.TriggerPassableWhenNecessary();
//            }
//        }
//
//        BallScript.instance.SetPassable(false);
//        enemyPassable = false;
//    }

    void StageCopy(List<GameObject> _stageP)
    {
        if (SelectStageList.Count > 0)
        {
            Debug.LogError("ステージ情報が残ったままです！！！一度リストをクリアしてください！");
        }

        for (int sCnt = 0; sCnt < _stageP.Count; sCnt++)
        {
            SelectStageList.Add(_stageP[sCnt]);
        }

//        Debug.Log("ステージ情報コピー完了");
    }
}