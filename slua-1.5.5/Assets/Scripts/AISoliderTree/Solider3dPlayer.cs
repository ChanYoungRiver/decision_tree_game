using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;
using UnityEngine.AI;
using DG.Tweening;
//using UnityEngine.Rigidbody;

public class Solider3dPlayer:SoliderPlayer {


    public Transform m_transform;//士兵的transform
    private Animator animator;//动画组件
    private NavMeshAgent agent;//寻路组件
    private LineSight ThisLineSight;//视野
    Vector3 originalPosition;//士兵初始位置
    Vector3 aPosition = Vector3.zero;//巡逻点A的位置
    Vector3 bPosition = Vector3.zero;//巡逻点B的位置
    Vector3 currentDestination = Vector3.zero;//士兵现在要巡逻的位置
    private GameObject enemy;//敌人
    private float shootTimer = 0.9f;//射击间隔时间器
    public Transform muzzlePoint; // 枪口的Transform组件
    public LayerMask layer; // 射击时射线能射到的碰撞层  
    public Transform fx; // 射中目标后的粒子效果


    //	private List<GameObject> bulletList = new List<GameObject> ();

    public Tweener moveRoundTweener;
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        ThisLineSight = GetComponent<LineSight>();

    }
    // Use this for initialization
    void Start () {
        m_transform = this.transform;
        m_playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();//导航代理
        enemy = GameObject.FindGameObjectWithTag("enemy");
        originalPosition = m_transform.position;   //士兵出生点位置（视为基地)  
        aPosition = new Vector3(-28, originalPosition.y, -37); //巡逻位置A  
        bPosition = new Vector3(34, originalPosition.y, 32); //巡逻位置B  
        currentDestination = bPosition;
        muzzlePoint = GameObject.Find("MP40GermanmachineGun").GetComponent<Transform>();//枪

    }
	
	// Update is called once per frame
	void Update () {

	}
    public void AIPatrol()
    {
        agent.SetDestination(currentDestination);
        animator.SetBool("Walk", true);
        if (Vector3.Distance(transform.position, currentDestination) <= 0.5f)
        {
            if (currentDestination == aPosition)//改变巡逻点
            {
                currentDestination = bPosition;
            }
            else
            {
                currentDestination = aPosition;
            }
        }
    }
    //巡逻
    public void MoveAround()
	{
        AIPatrol();
		/*RectTransform origialRect = this.GetComponent<RectTransform> ();

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
		moveRoundTweener.SetEase (Ease.Linear);*/
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
    public override bool NearbyExistEnemy(){
		return ThisLineSight.CanSeeTarget;
	}

    //攻击敌人
    public void AttackEnemy(ActionSoliderAttack bNodeAction){
        animator.Play("Shoot");
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            //SetAmmo(1);
            shootTimer = 0.9f;
            // 用一个RaycastHit对象保存射线的碰撞结果  
            RaycastHit info;
            // 从枪口所在位置向摄像机面向的正前方发出一条射线  
            // 该射线只与layer指定的层发生碰撞  
            if (Physics.Raycast(muzzlePoint.position, transform.TransformDirection(Vector3.forward), out info, 100, layer))
            {
                // 判断是否射中Tag为enemy的物体  
                if (info.transform.tag.Equals("enemy"))
                {
                    // 敌人减少生命  
                    info.transform.GetComponent<Enemy>().OnDamage(1);
                }
            }
            // 在射中的地方释放一个粒子效果  
            Debug.Log(fx +" "+ info.point +" "+ info.transform.rotation);
            Instantiate(fx, info.point, info.transform.rotation);

        }
        //.1创建子弹
        /*GameObject bulletObject = Instantiate(Resources.Load ("Prefab/Bullet")) as GameObject;
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
        */
    }

    public void StopAllAction(){
        agent.isStopped = true;
        animator.SetBool("Idle", true);

        /*if (moveRoundTweener != null) {
			moveRoundTweener.Kill ();
			moveRoundTweener = null;
		}*/

    }

    public void GoHome(){
        agent.isStopped = false;
        agent.SetDestination(originalPosition);//跑回到基地
        animator.SetBool("Run", true);
        if (Vector3.Distance(transform.position, originalPosition) <= 1.5f)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", true);
            //SetLife(999);//生命重置
            agent.isStopped = false;
        }
       /* RectTransform origialRect = this.GetComponent<RectTransform> ();

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
        */
	}

}
