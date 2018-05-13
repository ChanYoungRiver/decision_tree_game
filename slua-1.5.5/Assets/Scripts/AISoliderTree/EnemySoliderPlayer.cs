using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemySoliderPlayer : SoliderPlayer {

//	public int hp = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Transform labObj = this.transform.Find ("lab_hp");
		labObj.GetComponent<Text> ().text = "HP:"+hp;

	}

	void OnTriggerEnter2D(Collider2D collider2D){
		//		Debug.Log ("BulletScript1"+collider2D.gameObject.name); 
		BulletScript bullet = collider2D.GetComponent<BulletScript> ();
		if (bullet != null) {
//			Debug.Log ("bullet"+collider2D.gameObject.name); 
			hp = hp - bullet.damage;
			if (hp <= 0) {
				this.gameObject.SetActive (false);
			}
		}
	}
}
