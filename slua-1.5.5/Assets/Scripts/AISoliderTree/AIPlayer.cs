using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.AIBehaviorTree;
using DG.Tweening;

public enum AIPlayerGroup
{
	A,
	B,
	C,
}

public class AIPlayer : BInput {

	public AIPlayerGroup belongGroup= AIPlayerGroup.A;//所属队伍
	protected float speed = 300.0f;
	public int hp = 100;
	public int ammoNum = 5;

	public bool isDieActionRun = false;//是否在执行死亡动作
	public bool isMoveToHome = false;//是否在回家中
	public float shootSpace = 1.0f;//射击间隔
	public float loadAmmoTime = 2.0f;//填装子弹时间
	public float resetTime = 2.0f;//休息时间
	public bool isMove = false;
	public bool isDie = false;

	public GameObject inViewEnemyObj;//在视野范围内的敌人
	public GameObject inShootDistanceEnemyObj;//在射击范围的敌人

	private int chaseCell = 20;//追击的单位

	public Vector3 moveTarget;

	private GameLauncher4 gameLauncher4;

	protected Tweener moveRoundTweener;

	void Start () {
		gameLauncher4 = GameLauncher4.GetInstance();
		initConfig ();
	}

	public virtual void initConfig(){
		belongGroup = AIPlayerGroup.A;
	}

	// Update is called once per frame
	void Update () {
//		if (moveTarget != null) {
//			Debug.DrawLine(this.transform.position, moveTarget,Color.yellow);
//		}

		Transform labObj = this.transform.Find ("lab_hp");
		labObj.GetComponent<Text> ().text = "HP:"+hp;

	}


	public void Die(){
		isDie = true;
	}

	public bool IsBackHome(){
		return false;
	}

	public void MoveBackHome(){
		isMoveToHome = true;
	}
	//敌人是否在视野范围内
	public bool isEnemyInView(){

		foreach (GameObject enemy in gameLauncher4.aiPlayerList) 
		{
			AIPlayer aIPlayer = enemy.GetComponent<AIPlayer>();
			if (aIPlayer.belongGroup != this.belongGroup ) {
				float dis = (enemy.transform.position-this.transform.position).magnitude ;//计算两点间的距离
				if (dis <= 200.0f) {
					Debug.LogError("存在敌人");
					inViewEnemyObj = enemy;
					return true;
				}
			}
		}
		inViewEnemyObj = null;
		return false;
	}

	//敌人在射击范围内
	public bool isEnemyInShootDistance(){

		foreach (GameObject enemy in gameLauncher4.aiPlayerList) 
		{
			AIPlayer aIPlayer = enemy.GetComponent<AIPlayer>();
			if (aIPlayer.belongGroup != this.belongGroup) {
				float dis = (enemy.transform.position-this.transform.position).magnitude ;//计算两点间的距离
				if (dis <= 110.0f) {
					stopMove ();
					inShootDistanceEnemyObj = enemy;
					Debug.LogError("敌人在射击范围内");
					return true;
				}
			}

		}
		inShootDistanceEnemyObj = null;
		return false;
	}

	//执行射击
	public void Shoot(){

		if (inShootDistanceEnemyObj != null) {
			ammoNum--;

			refreshStateLabel ("执行射击 子弹数量:"+ammoNum);

			//.1创建子弹
			GameObject bulletObject = Instantiate(Resources.Load ("Prefab/Bullet")) as GameObject;

			bulletObject.GetComponent<BulletScript>().bulleGroup = belongGroup;
			bulletObject.transform.SetParent (  this.transform );
			bulletObject.transform.localPosition = new Vector3 (0,0,0);
			gameLauncher4.pushNewBullet (bulletObject);

			bulletObject.GetComponent<BulletScript> ().RuningBullet ( this.transform,inShootDistanceEnemyObj.transform );

		}


	}
	//装填子弹
	public void AddAmmo(){

		refreshStateLabel ("装填子弹");
		StartCoroutine (WaitAndAdmmo ());
	}

	private IEnumerator WaitAndAdmmo()  
	{  
		yield return new WaitForSeconds(loadAmmoTime);  
		ammoNum = 5;
	} 

	private void stopMove(){
		if (moveRoundTweener != null) {
			moveRoundTweener.Kill ();
			moveRoundTweener = null;
		}
	}

	//追击敌人
	public void Pursue(){
		Debug.LogError ("追击敌人");
		if(inViewEnemyObj!=null){//追击到可射击的范围内
			Vector3 pursueDir = inViewEnemyObj.gameObject.transform.position - this.transform.position;//追击方向
			moveTarget = this.transform.position + pursueDir.normalized*chaseCell;
			MoveToTarget ();
		}

	}

	//移动到指定地点
	public void MoveToTarget(){

		stopMove ();

		Transform origialTrans = this.GetComponent<RectTransform> ();

		float dis = (origialTrans.position-moveTarget).magnitude ;//计算两点间的距离

		isMove = true;

		Debug.Log ("origialTrans.position:"+origialTrans.position+" moveTarget"+moveTarget);

		moveRoundTweener = this.GetComponent<RectTransform> ().DOMove( moveTarget , dis/speed );
		moveRoundTweener.OnComplete (() => {
			moveRoundTweener = null;
			isMove = false;
		});

	}

	public void rest(){

		refreshStateLabel ("空闲中");
	}

	public void RandomMove(){
		refreshStateLabel ("随机移动");

		Transform origialTrans = this.GetComponent<RectTransform> ();
//		float x_ = origialTrans.position.x;
//		float y_ = origialTrans.position.y;

		float width = 1136.0f;
		float height = 640.0f;

//		RaycastHit hitInfo;
		float dirX;
		float dirY;
		Vector2 dir;
		float m_GroundCheckDistance = 200.0f;
//		float tX;
//		float tY;
		Vector3 targetTrans;
		do {
			dirX = Random.Range (-1.0f, 1.0f);
			dirY = Random.Range (-1.0f, 1.0f);
			dir = new Vector2 (dirX, dirY);

			targetTrans = origialTrans.position + ( (Vector3)dir).normalized  *m_GroundCheckDistance;
		} //while(!Physics2D.Raycast (new Vector2 (x_, y_), dir.normalized, m_GroundCheckDistance));
		while
		(
				! ( targetTrans.x > 0 && targetTrans.x < width && targetTrans.y > 0 && targetTrans.y < height)
		);


		targetTrans = origialTrans.position + ( (Vector3)dir).normalized  *m_GroundCheckDistance ;

		moveTarget = targetTrans;

		MoveToTarget ();
	}
		

	void OnTriggerEnter2D(Collider2D collider2D){
		//		Debug.Log ("BulletScript1"+collider2D.gameObject.name); 
		BulletScript bullet = collider2D.GetComponent<BulletScript> ();
		if (bullet != null && bullet.bulleGroup != belongGroup ) {
			//			Debug.Log ("bullet"+collider2D.gameObject.name); 
			hp = hp - bullet.damage;
			if (hp <= 0) {
				this.gameObject.SetActive (false);
			}
		}
	}

	private void refreshStateLabel(string str)
	{
		foreach (var child in this.gameObject.GetComponentsInChildren<Component>()) 
		{
			if (child.name == "lab_state") {
				child.GetComponent<Text> ().text = str;
			}

		}
	}

}
