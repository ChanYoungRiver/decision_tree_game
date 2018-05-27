using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpaceScript : MonoBehaviour {

	// Use this for initialization
	public bool isEnemyInSight{ get;set;}
	public List<EnemySoliderPlayer> inSightEnemyList = new List<EnemySoliderPlayer> ();
	void Start () {
//		Debug.Log ("SafeSpaceScript");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void OnCollisionEnter2D(Collision2D collision)
//	{
//		Debug.Log ("OnCollisionEnter2D"+collision.gameObject.name);  
//	}
//
//	void OnCollisionExit2D(Collision2D collision)
//	{
//		Debug.Log ("OnCollisionExit2D"+collision.gameObject.name);  
//	}
	void OnTriggerEnter2D(Collider2D collider2D){
		
		EnemySoliderPlayer enemyPlayer = collider2D.GetComponent<EnemySoliderPlayer> ();
		if (enemyPlayer != null) {
//			Debug.LogError ("OnTriggerEnter2D:"+collider2D.gameObject.name); 
			isEnemyInSight = true;
			inSightEnemyList.Add (enemyPlayer);
		}
	}

	void OnTriggerExit2D(Collider2D collider2D){
		
		EnemySoliderPlayer enemyPlayer = collider2D.GetComponent<EnemySoliderPlayer> ();
		if (enemyPlayer != null) {
//			Debug.Log ("OnTriggerExit2D"+collider2D.gameObject.name); 
			isEnemyInSight = false;
			inSightEnemyList.Remove (enemyPlayer);
		}
	}
}
