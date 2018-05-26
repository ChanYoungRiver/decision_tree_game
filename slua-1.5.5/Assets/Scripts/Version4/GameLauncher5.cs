using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher5 : MonoBehaviour {


	private static GameLauncher5 instance;
	public List<GameObject> aiPlayerList;
	// Use this for initialization
	void Start () {

//		initEnemy ();
		BeginGame ();
	}

	public virtual void initEnemy(){

		aiPlayerList.Clear ();
		//创建队伍A
		//		foreach (GameObject enemyPos in enemyListPostion) {
		GameObject aiObject = Instantiate(Resources.Load ("Prefab/V4/SoldierA")) as GameObject;
//		aiObject.transform.SetParent (  GameObject.Find("GameView").transform );
		aiObject.transform.position = new Vector3(-61.89f,0,65.72f);
		aiPlayerList.Add (aiObject);
		//		}


		//创建队伍B
//		foreach (GameObject enemyPos in enemyListPostion) {
//			GameObject enemyObject = Instantiate(Resources.Load ("Prefab/EnemyPointB")) as GameObject;
//			enemyObject.transform.SetParent (  GameObject.Find("GameView").transform );
//			enemyObject.transform.localPosition = enemyPos.transform.localPosition;
//			aiPlayerList.Add (enemyObject);
//			enemyPos.SetActive (false);
//		}

	}

	// Update is called once per frame
	void Update () {
		
	}

	public void BeginGame(){
//		GameObject.Find ("btn_begin").SetActive (false);
		initEnemy ();
		Debug.LogError ("开始游戏");
		foreach (GameObject enemy in aiPlayerList) {
			enemy.GetComponent<AIController> ().LoadTree ();
		}
//		isBegin = true;
	}
}
