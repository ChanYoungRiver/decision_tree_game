using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher5 : MonoBehaviour {


	private static GameLauncher5 instance;
	public List<GameObject> aiPlayerList;
	public List<GameObject> enemyListPostion;
	public List<GameObject> buffetList;

	string[] nameList = {"daniu","erhu"};

	public static int MANX_INDEX = 0;
	// Use this for initialization
	void Start () {

//		initEnemy ();
		BeginGame ();
		instance = this;
	}

	public static GameLauncher5 GetInstance(){
		return instance;
	}

	public static int getManIndex(){
		return MANX_INDEX++;
	}

	public virtual void initEnemy(){

		aiPlayerList.Clear ();
		//创建队伍A
		//		foreach (GameObject enemyPos in enemyListPostion) {
		GameObject aiObject = Instantiate(Resources.Load ("Prefab/V4/SoldierA")) as GameObject;
		aiObject.transform.position = new Vector3(-61.89f,0,65.72f);
		aiPlayerList.Add (aiObject);
		//		}
			AI3DPlayer aIPlayer1 = aiObject.transform.GetComponent<AI3DPlayer>();
			aIPlayer1.oriPosition = GameObject.Find("orig");
			aIPlayer1.soldierName = nameList[1];
			aIPlayer1.NameLabel();

		//创建队伍B
		foreach (GameObject enemyPos in enemyListPostion) {
			GameObject enemyObject = Instantiate(Resources.Load ("Prefab/V4/SoldierB")) as GameObject;
			enemyObject.transform.position = enemyPos.transform.position;
			aiPlayerList.Add (enemyObject);
			enemyPos.SetActive (false);

			AI3DPlayer aIPlayer = enemyObject.transform.GetComponent<AI3DPlayer>();
			aIPlayer.oriPosition = GameObject.Find("GroupBBase");
			aIPlayer.soldierName = nameList[1];
			aIPlayer.NameLabel();
		}

		foreach(GameObject obj in aiPlayerList)
		{
			AI3DPlayer aIPlayer = obj.transform.GetComponent<AI3DPlayer>();
			string fabName;
			if (aIPlayer.belongGroup == AIPlayerGroup.A) {
				fabName = "Prefab/GroupAHp";
			} else {
				fabName = "Prefab/GroupBHp";
			}

			GameObject hpObject = Instantiate(Resources.Load (fabName)) as GameObject;
			hpObject.transform.SetParent (GameObject.Find("HeadList").transform );
			GroupItemHP item = hpObject.transform.GetComponent<GroupItemHP> ();
			item.player = aIPlayer;
			item.updateHp ();

		}

	}

	// Update is called once per frame
	void Update () {

		bool isNeedUpdateView = false;
		foreach(GameObject obj in aiPlayerList)
		{
			AI3DPlayer aIPlayer = obj.transform.GetComponent<AI3DPlayer>();
			if (aIPlayer.isDie) 
			{
				aiPlayerList.Remove (aIPlayer.gameObject);
				Destroy (aIPlayer.gameObject);
				isNeedUpdateView = true; 
				break;
			}
		}

		if (isNeedUpdateView)
		{
			foreach(GameObject obj in aiPlayerList)
			{
				AI3DPlayer aIPlayer = obj.transform.GetComponent<AI3DPlayer>();
				aIPlayer.updateView();
			}
		}


		foreach(GameObject obj in buffetList)
		{
			if (obj.GetComponent<BulletScript3D> ().isFinish)
			{
				buffetList.Remove (obj);
				Destroy (obj);
				break;
			}
		}


	}

	public void BeginGame(){
//		GameObject.Find ("btn_begin").SetActive (false);
		buffetList = new List<GameObject>();
		initEnemy ();
		Debug.LogError ("开始游戏");
		foreach (GameObject enemy in aiPlayerList) {
			enemy.GetComponent<AIController> ().LoadTree ();
		}
//		isBegin = true;
	}
}
