using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour {

	// Use this for initialization
	private static GameLauncher instance;
	public List<GameObject> enemyList;

	public List<GameObject> enemyListPostion;

	void Start () {
		enemyList = new List<GameObject> ();
		initEnemy ();
		instance = this;
	}

	public static GameLauncher GetInstance(){
		return instance;
	}

	public virtual void initEnemy(){
		
		enemyList.Clear ();
		foreach (GameObject enemyPos in enemyListPostion) {
			GameObject enemyObject = Instantiate(Resources.Load ("Prefab/EnemyPoint")) as GameObject;
			enemyObject.transform.SetParent (  GameObject.Find("GameView").transform );
			enemyObject.transform.localPosition = enemyPos.transform.localPosition;
			enemyList.Add (enemyObject);
			enemyPos.SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {

		if(enemyList.Count>0){
			foreach (GameObject enemy in enemyList) {
				if (enemy.GetComponent<EnemySoliderPlayer> ().hp <= 0) {
					enemyList.Remove (enemy);
					Destroy (enemy);
					break;
				}
			}
		}

	}

	public bool CheckExistEnemy(){
		return enemyList.Count>0;
	}

	//
	public bool TargetInCicle(Player mPlayer,float radius){
		return false;
	}

}
