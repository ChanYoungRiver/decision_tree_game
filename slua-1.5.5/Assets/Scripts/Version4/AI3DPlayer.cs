using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.AIBehaviorTree;
using DG.Tweening;
using UnityEngine.AI;

public class AI3DPlayer : AIPlayer {


	RouteManager routeManager;
	private NavMeshAgent agent;//寻路组件
	private Animator animator;//动画组件
//	private NavMeshAgent agent;//寻路组件
	private LineSightV4 ThisLineSight;//视野
	Vector3 currentDestination = Vector3.zero;//士兵现在要巡逻的位置
	Vector3 aPosition = Vector3.zero;//巡逻点A的位置
	Vector3 bPosition = Vector3.zero;//巡逻点B的位置

	RoutePoint routePoint;

//	protected GameLauncher4 gameLauncher4;

	void Start () {
		//gameLauncher4 = GameLauncher4.GetInstance();
		agent = GetComponent<NavMeshAgent>();//导航代理
		initConfig ();
		CreatePointLineList ();
	}

	public override void initConfig(){
		belongGroup = AIPlayerGroup.A;
	}

	// Update is called once per frame
	void Update () {
		//		if (moveTarget != null) {
		//			Debug.DrawLine(this.transform.position, moveTarget,Color.yellow);
		//		}

//		Transform labObj = this.transform.Find ("lab_hp");
//		labObj.GetComponent<Text> ().text = "HP:"+hp;
	}

	void Awake()
	{
		animator = this.GetComponent<Animator>();
		ThisLineSight = GetComponent<LineSightV4>();
	}

	public override void Die(){
		isDie = true;
	}

	public override bool IsBackHome(){
		return false;
	}

	public override void MoveBackHome(){
		isMoveToHome = true;
	}
	//敌人是否在视野范围内
	public override bool isEnemyInView(){
		Debug.LogError ("敌人是否在视野范围内"+ThisLineSight.CanSeeTarget);
		return ThisLineSight.CanSeeTarget;

	}

	//敌人在射击范围内
	public override bool isEnemyInShootDistance(){


		inShootDistanceEnemyObj = null;
		if (ThisLineSight.CanSeeTarget && ThisLineSight.enemyObj && Vector3.Distance (transform.position, ThisLineSight.enemyObj.transform.position) <= 11f) 
		{
			stopMove ();
			Debug.LogError("敌人在射击范围内");
			return true;	
		}

		return false;
	}

	//执行射击
	public override void Shoot(){

//		if (inShootDistanceEnemyObj != null) {
//			ammoNum--;
//
//			refreshStateLabel ("执行射击 子弹数量:"+ammoNum);
//
//			//.1创建子弹
//			GameObject bulletObject = Instantiate(Resources.Load ("Prefab/Bullet")) as GameObject;
//
//			bulletObject.GetComponent<BulletScript>().bulleGroup = belongGroup;
//			bulletObject.transform.SetParent (  this.transform );
//			bulletObject.transform.localPosition = new Vector3 (0,0,0);
//			gameLauncher4.pushNewBullet (bulletObject);
//
//			bulletObject.GetComponent<BulletScript> ().RuningBullet ( this.transform,inShootDistanceEnemyObj.transform );
//
//		}


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

	public override void stopMove(){
//		if (moveRoundTweener != null) {
//			moveRoundTweener.Kill ();
//			moveRoundTweener = null;
//		}
//		agent.SetDestination(currentDestination);
//		animator.SetBool("Walk", true);
		agent.enabled = false;
		isMove = false;

	}

	//追击敌人
	public override void Pursue(){
		Debug.LogError ("追击敌人");
//		if(inViewEnemyObj!=null){//追击到可射击的范围内
//			Vector3 pursueDir = inViewEnemyObj.gameObject.transform.position - this.transform.position;//追击方向
//			moveTarget = this.transform.position + pursueDir.normalized*chaseCell;
//			MoveToTarget ();
//		}

	}

	//移动到指定地点
	public override void MoveToTarget(){

		stopMove ();

		isMove = true;
		agent.enabled = true;
		agent.SetDestination(currentDestination);
		animator.SetBool("Walk", true);
		if (Vector3.Distance(transform.position, currentDestination) <= 0.5f)
		{
			isMove = false;
		}

		if (routePoint!=null && Vector3.Distance(transform.position, routePoint.pointPos) <= 0.5f)
		{
			routePoint.isPass = true;
		}


	}

	public override void rest(){

		refreshStateLabel ("空闲中");
	}
		
	void OnTriggerEnter2D(Collider2D collider2D){
		//		Debug.Log ("BulletScript1"+collider2D.gameObject.name); 
//		BulletScript bullet = collider2D.GetComponent<BulletScript> ();
//		if (bullet != null && bullet.bulleGroup != belongGroup ) {
//			//			Debug.Log ("bullet"+collider2D.gameObject.name); 
//			hp = hp - bullet.damage;
//			if (hp <= 0) {
//				this.gameObject.SetActive (false);
//			}
//		}
	}

	public override void refreshStateLabel(string str)
	{
//		foreach (var child in this.gameObject.GetComponentsInChildren<Component>()) 
//		{
//			if (child.name == "lab_state") {
//				child.GetComponent<Text> ().text = str;
//			}
//
//		}
		GameObject.Find("lab_state1").GetComponent<Text>().text = str;
	}


	//随机移动,按照路线巡逻
	public override void RandomMove()
	{
		if ( isEnemyInView() ){
			stopMove();
		}else{
			refreshStateLabel ("随机移动");
			Debug.Log ("随机移动");
			RoutePoint routePoint = routeManager.getCurPoint ();
			currentDestination = routePoint.pointPos;
			MoveToTarget ();	
		}



	}


	public void CreatePointLineList(){
		GameObject routeListObj =  GameObject.Find ("RouteList");
		routeManager = new RouteManager ();
		int i=0;  
		while(i<routeListObj.transform.childCount){  
			Transform parent=routeListObj.transform.GetChild(i);  
			RoutePointList routePointList = new RoutePointList ();
			Debug.LogError ("child: "+i+" "+parent.name); 
			int j = 0;
			while (j < parent.transform.childCount) {
				Transform childNode=parent.transform.GetChild(j);  
				Debug.LogError ("child: "+j+" "+childNode.name); 
				RoutePoint routePoint = new RoutePoint ();
				routePoint.pointPos = childNode.position;
				routePointList.pointList.Add (routePoint);
				j++;
			}
			i++;
			routeManager.routeList.Add (routePointList);
		}  
	}

}
