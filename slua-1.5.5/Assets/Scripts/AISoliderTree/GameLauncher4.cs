using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher4 : MonoBehaviour {

	// Use this for initialization
	private static GameLauncher4 instance;
	public List<GameObject> aiPlayerList;

	public List<GameObject> enemyListPostion;

	public List<GameObject> bulletList;//子弹管理

	private bool isBegin = false;

	void Start () {
		aiPlayerList = new List<GameObject> ();
		bulletList = new List<GameObject> ();
		instance = this;
	}

	public static GameLauncher4 GetInstance(){
		return instance;
	}

	public virtual void initEnemy(){

		aiPlayerList.Clear ();
		//创建队伍A
//		foreach (GameObject enemyPos in enemyListPostion) {
			GameObject aiObject = Instantiate(Resources.Load ("Prefab/EnemyPointA")) as GameObject;
			aiObject.transform.SetParent (  GameObject.Find("GameView").transform );
			aiObject.transform.localPosition = GameObject.Find("basePos").transform.localPosition;
			aiPlayerList.Add (aiObject);
//		}


		//创建队伍B
		foreach (GameObject enemyPos in enemyListPostion) {
			GameObject enemyObject = Instantiate(Resources.Load ("Prefab/EnemyPointB")) as GameObject;
			enemyObject.transform.SetParent (  GameObject.Find("GameView").transform );
			enemyObject.transform.localPosition = enemyPos.transform.localPosition;
			aiPlayerList.Add (enemyObject);
			enemyPos.SetActive (false);
		}

	}

	// Update is called once per frame
	void Update () {

		if (!isBegin)
			return;

		if(aiPlayerList.Count>0){
			foreach (GameObject enemy in aiPlayerList) {
				if (enemy.GetComponent<AIPlayer> ().hp <= 0) {
					aiPlayerList.Remove (enemy);
					Destroy (enemy);
					break;
				}
			}
		}

		if(bulletList.Count>0){
			foreach (GameObject bullet in bulletList) {
				if (bullet.GetComponent<BulletScript> ().isFinish == true) {
					bulletList.Remove (bullet);
					Destroy (bullet);
					break;
				}
			}
		}


	}

	public void pushNewBullet(GameObject bullet){
		bulletList.Add (bullet);
	}

	public void BeginGame(){
		GameObject.Find ("btn_begin").SetActive (false);
		initEnemy ();
		Debug.LogError ("开始游戏");
		foreach (GameObject enemy in aiPlayerList) {
			enemy.GetComponent<AIController> ().LoadTree ();
		}
		isBegin = true;
	}

}
