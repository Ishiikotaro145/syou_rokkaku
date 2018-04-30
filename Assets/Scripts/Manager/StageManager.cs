using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum STAGE:int 
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




public class StageManager : SingletonBase<StageManager> {

	public List<GameObject> pStage1_1;
	public List<GameObject> pStage1_2;
	public List<GameObject> pStage1_3;
	public List<GameObject> pStage2_1;
	public List<GameObject> pStage2_2;
	public List<GameObject> pStage2_3;
	public List<GameObject> pStage3_1;
	public List<GameObject> pStage3_2;
	public List<GameObject> pStage3_3;

	List<GameObject> SelectStageList = new List<GameObject>();

	int nowStage = -1;
	int nowWave = 0;
	int maxWaveCnt = 0;


	void Awake()
	{
		if (this != GetInstance)
		{
			Destroy (this);
			return;
		}
		DontDestroyOnLoad (this.gameObject);
	}


	// Use this for initialization
	void Start () {
		nowStage = (int)STAGE.STAGE1_1;
		switch(nowStage)
		{
		case (int)STAGE.STAGE1_1:
			maxWaveCnt = pStage1_1.Count;
			StageCopy (pStage1_1);
			break;
		case (int)STAGE.STAGE1_2:
			maxWaveCnt = pStage1_2.Count;
			StageCopy (pStage1_2);
			break;
		case (int)STAGE.STAGE1_3:
			maxWaveCnt = pStage1_3.Count;
			StageCopy (pStage1_3);
			break;
		case (int)STAGE.STAGE2_1:
			maxWaveCnt = pStage2_1.Count;
			StageCopy (pStage2_1);
			break;
		case (int)STAGE.STAGE2_2:
			maxWaveCnt = pStage2_2.Count;
			StageCopy (pStage2_2);
			break;
		case (int)STAGE.STAGE2_3:
			maxWaveCnt = pStage2_3.Count;
			StageCopy (pStage2_3);
			break;
		case (int)STAGE.STAGE3_1:
			maxWaveCnt = pStage3_1.Count;
			StageCopy (pStage3_1);
			break;
		case (int)STAGE.STAGE3_2:
			maxWaveCnt = pStage3_2.Count;
			StageCopy (pStage3_2);
			break;
		case (int)STAGE.STAGE3_3:
			maxWaveCnt = pStage3_3.Count;
			StageCopy (pStage3_3);
			break;
		}
		nowWave = 0;
		Debug.Log ("ステージマネージャーStart");




	}




	
	// Update is called once per frame
	void Update () {
		if(EnemyManager.GetInstance.GetNowEnemyCnt() <= 0)
		{
			if (nowWave >= maxWaveCnt) {
				//ステージクリア

			} 
			else 
			{


				//次のWAVEの敵を出す。
				GameObject stageObject = (GameObject)Instantiate 
					(
						SelectStageList[nowWave],
						transform.position,
						Quaternion.identity
					);
				nowWave++;

				Debug.Log ("Waveエネミー生成完了");
			}
		}



	}


	void StageCopy(List<GameObject> _stageP)
	{
		if (SelectStageList.Count > 0) 
		{
			Debug.LogError ("ステージ情報が残ったままです！！！一度リストをクリアしてください！");
		}

		for (int sCnt = 0; sCnt < _stageP.Count; sCnt++)
		{
			SelectStageList.Add (_stageP[sCnt]);
		}

		Debug.Log ("ステージ情報コピー完了");
	}
}
