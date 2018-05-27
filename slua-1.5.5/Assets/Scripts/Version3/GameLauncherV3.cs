using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncherV3 : GameLauncher {


	public override void initEnemy(){
		Debug.LogError ("GameLauncherV3");
		enemyList.Clear ();
		foreach (GameObject enemyPos in enemyListPostion) {
			GameObject enemyObject = Instantiate(Resources.Load ("Prefab/HightLvEnemy/EnemyPoint_Lv1")) as GameObject;
			enemyObject.transform.SetParent (  GameObject.Find("GameView").transform );
			enemyObject.transform.localPosition = enemyPos.transform.localPosition;
			enemyList.Add (enemyObject);
			enemyPos.SetActive (false);
		}

	}


}
