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
    BATTLE,
    CLEAR,
    BACK_OVERRAY,
    W0,
    W1,
    W2,
    W3,
    W4,
    W5,
    W6,
    W7,
    W8,
    W9,
    SLASH,
    BACKBLACK,
    STAGE,
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
    List<GameObject> numList = new List<GameObject> ();
    List<GameObject> stageNumList = new List<GameObject> ();
    private GameObject allEnemyInStage;

    private bool gameStart;
    int nowStage = -1;
    int nowWave = 0;
    int maxWaveCnt = 0;


//    private bool enemyPassable;

    //WAVE STARTの表示に必要なもの
    const float cNumeratorScale = 1.5f;

    private bool isWaveStart = false;
    float  waveStartPos = 0.0f;
    bool isWaveStartBay = false;
    float waveStartAlpha = 0.0f;

    float numeratorAlpha = 0.0f;
    float numeratorScale = 1.5f;
    bool isNumeratorDraw = false;




    //WAVE CLEARの表示に必要な画像
    const float waveClearDistance = 0.2f;
    private bool isWaveClear = false;
    int waveClearTime = 0;
    float overAlpha = 0.5f;
    float overScale = 2.0f;
    float waveClearPosX = waveClearDistance;
    float waveClearPosXConst = waveClearDistance;
    GameObject waveObject;

    //Stage数やその辺の情報



    //
    public int GetNowStage(){ return nowStage; }

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
        waveStartPos = -10.0f;
        isWaveStartBay = false;





    }


    public void SetWaveStartBay()
    {
        isWaveStartBay = true;
        waveStartPos = 0.1f;
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


    void Start()
    {

        //必要なインスタンス生成
        for (int cnt = 0; cnt < 10; cnt++) 
        {
            GameObject num = (GameObject)Instantiate 
                (
                    pWaveClear[3 + cnt],
                    transform.position,
                    Quaternion.identity
                );
            numList.Add (num);

            GameObject num2 = (GameObject)Instantiate 
                (
                    pWaveClear[3 + cnt],
                    transform.position,
                    Quaternion.identity
                );
            num2.transform.localScale = new Vector3 (0.15f,0.15f,0.15f);
            stageNumList.Add (num2);
        }
        waveObject = (GameObject)Instantiate
            (
                pWaveClear[0],
                transform.position,
                Quaternion.identity
            );

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

        


        SpriteRenderer waveSprite = waveObject.GetComponent<SpriteRenderer>();
        var waveColor = waveSprite.color;
        waveColor.a = 1.0f - overAlpha;
        waveSprite.color = waveColor;
        waveObject.transform.position = new Vector3(-1.37f - (waveClearPosXConst - waveClearPosX), 0.0f, 0.0f);



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
            waveStartPos *= 0.9f;
            waveStartAlpha += 0.02f;
            if (waveStartAlpha >= 0.6f) 
            {
                waveStartAlpha = 0.6f; 
            }
            if(Mathf.Abs(waveStartPos) <= 0.1f)
            {
                isNumeratorDraw = true;
            }
        } 
        else
        {
            waveStartPos += 0.05f;
            waveStartPos *= 1.4f;
            if (waveStartPos >= 20.0f)waveStartPos = 20.0f;


            waveStartAlpha -= 0.02f;
            if (waveStartAlpha <= 0.0f)
            {
                waveStartAlpha = 0.0f;
                isNumeratorDraw = false;
                numeratorScale = 1.5f;
                numeratorAlpha = 0.0f;
            }
        }

        if (isNumeratorDraw == true)
        {
            numeratorScale *= 0.9f;
            numeratorAlpha += 0.05f;
            if (numeratorAlpha >= 1.0f)numeratorAlpha = 1.0f;
            //Debug.Log("アルファ値" + numeratorAlpha);
        }



        //WAVE
        SpriteRenderer waveSprite = pWaveClear [(int)WAVECLEAR.BATTLE].GetComponent<SpriteRenderer> ();
        var waveColor = waveSprite.color;
        waveColor.a = 1.0f;
        waveSprite.color = waveColor;
        pWaveClear [(int)WAVECLEAR.BATTLE].transform.position = new Vector3 (waveStartPos - 0.6f,0.0f,0.0f);

        //数字
        SpriteRenderer nowSprite = numList [nowWave].GetComponent<SpriteRenderer> ();
        var nowColor = nowSprite.color;
        nowColor.a = numeratorAlpha;
        nowSprite.color = nowColor;
        numList[nowWave].transform.position = new Vector3 (waveStartPos + 1.0f,0.15f,0.0f);
        //pWaveClear [nowWave + 3].transform.position = new Vector3 (waveStartPos + 1.0f,0.15f,0.0f);

        //スラッシュ
        SpriteRenderer slashSprite = pWaveClear [(int)WAVECLEAR.SLASH].GetComponent<SpriteRenderer> ();
        var slashColor = slashSprite.color;
        slashColor.a = 1.0f;
        slashSprite.color = slashColor;
        pWaveClear [(int)WAVECLEAR.SLASH].transform.position = new Vector3 (waveStartPos + 1.4f,0.0f,0.0f);



        //数値　母数
        SpriteRenderer maxSprite = pWaveClear [SelectStageList.Count + 3].GetComponent<SpriteRenderer> ();
        var maxColor = maxSprite.color;
        maxColor.a = 1.0f;
        maxSprite.color = maxColor;
        pWaveClear [SelectStageList.Count + 3].transform.position = new Vector3 (waveStartPos + 1.9f,-0.15f,0.0f);


        //現在のステージ数管理
        SpriteRenderer stageSprite = pWaveClear[(int)WAVECLEAR.STAGE].GetComponent<SpriteRenderer>();
        var stageColor = stageSprite.color;
        stageColor.a = 1.0f;
        stageSprite.color = stageColor;
        pWaveClear[(int)WAVECLEAR.STAGE].transform.position = new Vector3 (waveStartPos - 0.25f,1.0f,0.0f);
        //0
        SpriteRenderer zeroSprite = stageNumList[0].GetComponent<SpriteRenderer>();
        var zeroColor = zeroSprite.color;
        zeroColor.a = 1.0f;
        zeroSprite.color = zeroColor;
        stageNumList[0].transform.position = new Vector3 (waveStartPos + 0.55f,1.0f,0.0f);

        //Stage
        SpriteRenderer nowStageSprite = stageNumList[nowStage + 1].GetComponent<SpriteRenderer>();
        var nowStageColor = nowStageSprite.color;
        nowStageColor.a = 1.0f;
        nowStageSprite.color = nowStageColor;
        stageNumList[nowStage + 1].transform.position = new Vector3 (waveStartPos + 0.85f,1.0f,0.0f);







        //背景の黒画像
        SpriteRenderer backSprite = pWaveClear[(int)WAVECLEAR.BACKBLACK].GetComponent<SpriteRenderer>();
        var backColor = backSprite.color;
        backColor.a = waveStartAlpha;
        backSprite.color = backColor;






        if(waveStartPos < -10.0f)
        {
            isWaveStart = false;
        }
        //Debug.Log(waveStartPos);
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