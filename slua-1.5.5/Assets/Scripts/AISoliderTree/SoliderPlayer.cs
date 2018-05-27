using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;
using DG.Tweening;
//using UnityEngine.Rigidbody;

public enum SoliderGroup
{
	A,
	B,
	C,
}


public class SoliderPlayer : BInput {


	public SoliderGroup soliderGroup = SoliderGroup.A;

	public PlayerController m_playerController;
//	private Vector2 dir=Vector2.up;
	public int hp = 100;
	protected float speed = 300.0f;
	private SafeSpaceScript safeSpaceScript;
	private List<GameObject> bulletList = new List<GameObject> ();

	public Tweener moveRoundTweener;

	// Use this for initialization
	void Start () {
		m_playerController = GetComponent<PlayerController>();

		Transform safeSpace = this.transform.Find ("SafeSpace");
		safeSpaceScript = safeSpace.GetComponent<SafeSpaceScript> ();

	}
	
	// Update is called once per frame
	void Update () {

		if(bulletList.Count>0){
			foreach (GameObject bullet in bulletList) {
				if (bullet.GetComponent<BulletScript> ().tweener == null) {
					bulletList.Remove (bullet);
					Destroy (bullet);
					break;
				}
			}
		}

	}

	//巡逻
	public void MoveAround()
	{
		RectTransform origialRect = this.GetComponent<RectTransform> ();

		GameObject targetPontObj = m_playerController.GetCurPointObj();
		RectTransform curPontObjRect = targetPontObj.GetComponent<RectTransform> ();
		float targetX = curPontObjRect.position.x;
		float targetY = curPontObjRect.position.y;


		float dis = (origialRect.position-curPontObjRect.position).magnitude ;//计算两点间的距离

		if (moveRoundTweener != null) {
			moveRoundTweener.Kill ();
			moveRoundTweener = null;
		}

		moveRoundTweener = this.GetComponent<RectTransform> ().DOMove( new Vector3(targetX,targetY,0)  , dis/speed );
		moveRoundTweener.OnComplete (() => {
			moveRoundTweener = null;
			m_playerController.curPontIndex++;
		});
		moveRoundTweener.SetEase (Ease.Linear);
		Debug.Log ("Player:MoveAround");


		//		float x_ = this.GetComponent<RectTransform> ().position.x;
		//		float y_ = this.GetComponent<RectTransform> ().position.y;
		//(dir.y*30.0)


		//		dir = new Vector2 ( curPontObjRect.position.x - x_, curPontObjRect.position.y - y_  );
//		float y_ = this.GetComponent<RectTransform> ().localPosition.y;

//		this.GetComponent<RectTransform> ().DOLocalMoveY (y_+2,1.0f);

//		RectTransform rect = this.GetComponent<RectTransform> ();
//		RaycastHit hitInfo;
//		float m_GroundCheckDistance = 20.0f;
//		if (Physics2D.Raycast (  new Vector2(0,rect.position.y), Vector2.up, m_GroundCheckDistance)) {
//			dir = Vector2.down;
//		} else if (Physics2D.Raycast ( new Vector2(0,rect.position.y), Vector2.down, m_GroundCheckDistance)) {
//			dir = Vector2.up;
//		}
	
	}

	//范围内是否存在敌人
	public virtual bool  NearbyExistEnemy(){
		return safeSpaceScript.isEnemyInSight;
	}

	//攻击敌人
	public void AttackEnemy(ActionSoliderAttack bNodeAction){

		//.1创建子弹
		GameObject bulletObject = Instantiate(Resources.Load ("Prefab/Bullet")) as GameObject;
		bulletObject.transform.SetParent (  this.transform );
		bulletObject.transform.localPosition = new Vector3 (0,0,0);
		bulletList.Add (bulletObject);

		Transform safeSpacObj = this.transform.Find ("SafeSpace");
		List<EnemySoliderPlayer> inSightEnemyList = safeSpacObj.GetComponent<SafeSpaceScript> ().inSightEnemyList;

		//2.找到一个在视野内的敌人
		foreach(EnemySoliderPlayer enemyPlayer in inSightEnemyList){
			bulletObject.GetComponent<BulletScript> ().RuningBullet ( this.transform,enemyPlayer.transform );
			break;
		}

	}

	public void StopAllAction(){

		if (moveRoundTweener != null) {
			moveRoundTweener.Kill ();
			moveRoundTweener = null;
		}

	}

	public void GoHome(){
	

		RectTransform origialRect = this.GetComponent<RectTransform> ();

		GameObject targetPontObj = GameObject.Find ("basePos");
		RectTransform curPontObjRect = targetPontObj.GetComponent<RectTransform> ();
		float targetX = curPontObjRect.position.x;
		float targetY = curPontObjRect.position.y;


		float dis = (origialRect.position-curPontObjRect.position).magnitude ;//计算两点间的距离

		if (moveRoundTweener != null) {
			moveRoundTweener.Kill ();
			moveRoundTweener = null;
		}

		Tweener moveBackTweener = this.GetComponent<RectTransform> ().DOMove( new Vector3(targetX,targetY,0)  , dis/speed );
//		moveBackTweener.OnComplete (() => {
//			moveBackTweener = null;
//		});
		moveBackTweener.SetEase (Ease.Linear);

	}

	public virtual void Escape(){
		
	}

}
