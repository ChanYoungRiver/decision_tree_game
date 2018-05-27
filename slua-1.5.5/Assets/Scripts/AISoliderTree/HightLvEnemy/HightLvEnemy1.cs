using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HightLvEnemy1 : EnemySoliderPlayer {

	// Use this for initialization
	void Start () {
		soliderGroup = SoliderGroup.B;
		speed = 200.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//范围内是否存在敌人
	public override bool NearbyExistEnemy(){

		Transform safeSpace = this.transform.Find ("SafeSpace");
		BaseSafeSpaceScript safeSpaceScript = safeSpace.GetComponent<BaseSafeSpaceScript> ();

		return safeSpaceScript.isEnemyInSight;
	}

	public override void Escape(){

		Transform safeSpace = this.transform.Find ("SafeSpace");
		BaseSafeSpaceScript safeSpaceScript = safeSpace.GetComponent<BaseSafeSpaceScript> ();
		GameObject enemyInSightObj = safeSpaceScript.enemyInSightObj;
		if (enemyInSightObj != null) 
		{
			Transform origial = enemyInSightObj.transform;
			Transform target = this.transform;
			Vector2 dir = (Vector2)(target.position - origial.position);
			//逃跑方向
			dir = new Vector2( dir.x*100,dir.y*100  );
			Vector2 target_ = dir + (Vector2)origial.position;

//			//3.飞行距离
//			float dis = (origial.position-target.position).magnitude ;//计算两点间的距离
//			if (tweener!=null){
//				tweener.Kill ();
//			}
//			tweener = this.GetComponent<RectTransform> ().DOMove( new Vector3(target_.x,target_.y,0)  , dis/speed );
//			tweener.SetEase (Ease.Linear);
			RectTransform rect = this.GetComponent<RectTransform> ();
			RaycastHit hitInfo;
			float m_GroundCheckDistance = 40.0f;
			if (Physics2D.Raycast (  new Vector2(target.position.x,target.position.y), dir, m_GroundCheckDistance)) {
				dir = Vector2.down;
			} else if (Physics2D.Raycast ( new Vector2(0,rect.position.y), Vector2.down, m_GroundCheckDistance)) {
				dir = Vector2.up;
			}

			//Physics2D.Linecast

		}

	}

}
