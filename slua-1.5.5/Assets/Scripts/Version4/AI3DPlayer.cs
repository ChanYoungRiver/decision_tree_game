using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.AIBehaviorTree;
using DG.Tweening;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class AI3DPlayer : AIPlayer {

	public string manState="";

	protected int dangersHP = 100;
	RouteManager routeManager;
	private Animator animator;//动画组件
	private NavMeshAgent agent;//寻路组件
    public Transform m_transform;//士兵的transform
    private float shootTimer = 0.9f;//射击间隔时间器
    private LineSightV4 ThisLineSight;//视野
    public Transform muzzlePoint; // 枪口的Transform组件
    public LayerMask layer; // 射击时射线能射到的碰撞层  
    public Transform fx; // 射中目标后的粒子效果
	public int SOLIDER_INDEX;
    Vector3 currentDestination = Vector3.zero;//士兵现在要巡逻的位置

	public string routeGroup = "RouteListGroupA";

	public RoutePoint routePoint;

//	protected GameLauncher4 gameLauncher4;

	void Start () {
        //gameLauncher4 = GameLauncher4.GetInstance();
        m_transform = this.transform;
		SOLIDER_INDEX = GameLauncher5.getManIndex ();
        agent = GetComponent<NavMeshAgent>();//导航代理
		initConfig ();
		CreatePointLineList ();
        muzzlePoint = GameObject.Find("MP40GermanmachineGun").GetComponent<Transform>();//枪

		animator = this.GetComponent<Animator>();
		ThisLineSight = GetComponent<LineSightV4>();
		ThisLineSight.manGroup = belongGroup;
		ThisLineSight.solideIndex = SOLIDER_INDEX;
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
		if (routePoint!=null && Vector3.Distance(transform.position, routePoint.pointPos) <= 0.5f)
		{
			routePoint.isPass = true;
			stopMove ();
			routePoint = null;
		}

//		Transform labObj = this.transform.Find ("lab_hp");
//		labObj.GetComponent<Text> ().text = "HP:"+hp;

	}

	void Awake()
	{

	}

	public override void Die(){
		isDie = true;
		this.gameObject.SetActive (false);
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
    public override void Shoot()
    {
		refreshStateLabel ("射击");
    	if(ThisLineSight.enemyObj!=null)
        {
			Debug.LogError ("ThisLineSight.enemyObj."+ThisLineSight.enemyObj.name+" "+this.transform.name);
			Transform realPhy = ThisLineSight.enemyObj.transform.parent.Find("EyePoint");
			transform.LookAt(realPhy);

			Debug.LogError("shoot function");
			animator.SetBool("Walk",false);
			animator.SetBool("Run",false);
			animator.Play("Shoot");
			//animator.SetBool("Shoot", true);

			//		shootTimer -= Time.deltaTime;
			//        if (shootTimer <= 0)
			//        {
			//SetAmmo(1);
			//            shootTimer = 0.9f;
			// 用一个RaycastHit对象保存射线的碰撞结果  
			RaycastHit info;
			// 从枪口所在位置向摄像机面向的正前方发出一条射线  
			// 该射线只与layer指定的层发生碰撞  
			Transform origal = this.transform.Find ("EyePoint");
				

			bool hit = Physics.Raycast(origal.position, (realPhy.position-origal.position).normalized*1000  , out info, 1000, layer);

			Debug.Log(info.point);
			Debug.DrawLine (origal.position,(realPhy.position-origal.position).normalized*1000 );


			GameObject bulletObject = Instantiate(Resources.Load ("Prefab/Bullet3D")) as GameObject;

			bulletObject.transform.position = origal.position;
			bulletObject.GetComponent<BulletScript3D> ().bulleGroup = belongGroup;
			bulletObject.GetComponent<BulletScript3D> ().RuningBullet (origal.position, origal.position+(realPhy.position-origal.position).normalized*1000 );
			GameLauncher5.GetInstance ().buffetList.Add (bulletObject);

			if (hit)
			{
				
				Debug.LogError ("大众敌人了-1@"+info.collider.transform.name);
				// 判断是否射中Tag为enemy的物体  
				if (info.collider.transform.tag.Equals("EnemyPhy"))
				{
					Debug.LogError ("大众敌人了1");
					AI3DPlayer aIPlayer = info.transform.GetComponent<AI3DPlayer>();
					if(aIPlayer.belongGroup!=belongGroup)
					{
						aIPlayer.hp = aIPlayer.hp - 20;
						Debug.LogError ("大众敌人了2");
					}
					// 敌人减少生命  
					//info.transform.GetComponent<Enemy>().OnDamage(1);
					//                    Instantiate(fx, info.point, info.transform.rotation);
				}
				//            }
				// 在射中的地方释放一个粒子效果  

				Debug.Log(info.point);

			}

        }
     
        //装填子弹
    }
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
		if (isMove)
		{
			agent.isStopped = true;
			isMove = false;
		}


	}

	//追击敌人
	public override void Pursue(){

		refreshStateLabel ("追击敌人");
//		Debug.LogError ("追击敌人");
//		if(inViewEnemyObj!=null){//追击到可射击的范围内
//			Vector3 pursueDir = inViewEnemyObj.gameObject.transform.position - this.transform.position;//追击方向
//			moveTarget = this.transform.position + pursueDir.normalized*chaseCell;
//			MoveToTarget ();
//		}
		agent.isStopped = true;
        ThisLineSight.Sensitity = LineSightV4.SightSensitivity.LOOSE;
        if(ThisLineSight.enemyObj!=null)
        {
        	transform.LookAt(ThisLineSight.enemyObj.transform);
        }
        isMove = true;
       
        agent.SetDestination(ThisLineSight.LastKnowSighting);
         agent.isStopped = false;
        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);

	}

	//移动到指定地点
	public override void MoveToTarget(){

		stopMove ();
		agent.isStopped = false;
		isMove = true;
		agent.enabled = true;
		agent.SetDestination(currentDestination);
		animator.SetBool("Walk", true);


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
//		GameObject.Find("lab_state1").GetComponent<Text>().text = str;
		this.manState = str;
	}


	//随机移动,按照路线巡逻
	public override void RandomMove()
	{
		if ( isEnemyInView() ){
			stopMove();
		}else{
			refreshStateLabel ("随机移动");
			Debug.Log ("随机移动");
			routePoint = routeManager.getCurPoint ();
			currentDestination = routePoint.pointPos;
			MoveToTarget ();	
		}



	}


	public void CreatePointLineList(){
		GameObject routeListObj =  GameObject.Find (routeGroup);
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
				routePoint.pointName = childNode.name;
				routePointList.pointList.Add (routePoint);
				j++;
			}
			i++;
			routeManager.routeList.Add (routePointList);
		}  
	}

	public bool isDangerousState(){
		return hp <= dangersHP;
	}

}
