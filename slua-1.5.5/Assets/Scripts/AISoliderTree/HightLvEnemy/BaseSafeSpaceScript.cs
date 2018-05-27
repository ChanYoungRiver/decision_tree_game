using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSafeSpaceScript : MonoBehaviour {

	public bool isEnemyInSight{ get;set;}
	public GameObject enemyInSightObj;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider2D){


//		Debug.Log ("OnTriggerEnter2D"+collider2D.gameObject.name);
		foreach (var component in collider2D.gameObject.GetComponents<Component>()) 
		{
			
			SoliderPlayer player = component as SoliderPlayer;
			if (player!=null)
			{
				foreach (var myComponent in this.transform.parent.gameObject.GetComponents<Component>()) 
				{
					SoliderPlayer myPlayer = myComponent as SoliderPlayer;
					if (myPlayer!=null)
					{
						if (myPlayer.soliderGroup != player.soliderGroup) {
							isEnemyInSight = true;
							enemyInSightObj = collider2D.gameObject;
						}
					}

				}

					
			}

		}
			
	}

	void OnTriggerExit2D(Collider2D collider2D){


		foreach (var component in collider2D.gameObject.GetComponents<Component>()) 
		{

			SoliderPlayer player = component as SoliderPlayer;
			if (player!=null)
			{


				foreach (var myComponent in this.gameObject.GetComponents<Component>()) 
				{
					SoliderPlayer myPlayer = myComponent as SoliderPlayer;
					if (myPlayer!=null)
					{
						if (myPlayer.soliderGroup != player.soliderGroup) {
							isEnemyInSight = false;
							enemyInSightObj = null;
						}
					}

				}


			}

		}

	}

}
