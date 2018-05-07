using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour {
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject enemy;//敌人
    private GameObject Waypoint;//巡逻点
    private int playerState;//记录士兵当前状态

    public int lifeValue = 100;//生命值
    public int ammoNum = 6;//弹药数量
    public const int stateIdle = 0;//休息状态
    public const int stateWalk = 1;//行走状态
    public const int stateRun = 2;//奔跑状态
    public const float WarnDistance = 20.0f;//可追逐范围
    public const float attackDistance = 10.0f;//可攻击范围
    public Transform m_transform;
    GUIText lab_ammoNum;
    GUIText lab_aliveValue;
    Vector3 destination;
    
    CharacterController m_ch;//角色控制器组件

    float m_movSpeed = 3.0f;//角色移动速度

    float m_gravity = 2.0f;//重力

    Vector3 originalPosition;//士兵初始位置
    public Vector3[] WP;//所有路点 
    Vector3 newPosition = Vector3.zero;//巡逻点A的位置
    Vector3 currentDestination = Vector3.zero;//士兵现在要巡逻的位置

    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start() {
        playerState = stateIdle;
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();//导航代理
        enemy = GameObject.Find("Enemy");
        Waypoint = GameObject.Find("Wp1");
        //InitWayPoint();
        originalPosition = transform.position;   //拿到物体初始位置  
        float x = 7;// Random.Range(-20, 20);
        float z = -4;//Random.Range(-20, 20);
        newPosition =new Vector3 (x, originalPosition.y, z); //巡逻位置A  
        currentDestination = newPosition;

    }
    // Update is called once per frame
    void Update() {
        //如果生命为0，什么也不做
        if (lifeValue <= 0)
            return;

        AttackDistance();//判断敌人距离

       

        /*       float h = Input.GetAxis("Horizontal");
               float v = Input.GetAxis("Vertical");
               AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
               if (stateInfo.shortNameHash == Animator.StringToHash("Run"))
               {
                   if (Input.GetButton("Fire1"))
                       animator.SetBool("Shoot", true);
               }
               else
               {
                   animator.SetBool("Shoot", false);
                   animator.SetFloat("Speed", h * h + v * v);
                   animator.SetFloat("Direction", h, 0.25f, Time.deltaTime);
               }*/
    }
    public void ChangeRoad()
    {

        if(currentDestination == newPosition)
        {
            currentDestination = originalPosition;
        }
        else
        {
            currentDestination = newPosition;
        }
    }

    //判断敌人与主角的距离
    void AttackDistance()
    {
        if (Vector3.Distance(transform.position, enemy.transform.position) <= WarnDistance)
        {
            animator.SetBool("Run", true);
            if (Vector3.Distance(transform.position, enemy.transform.position) <= attackDistance)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                if (Vector3.Angle(direction, transform.forward) < 2)
                {
                    animator.SetBool("Shoot", true);
                    print("我已经打到你了！" + Random.value);
                }
                
            }           
            playerState = stateRun;
            m_transform.LookAt(enemy.transform);//面朝敌人
            m_transform.Translate(Vector3.forward * Time.deltaTime);
                       
        }
        //随机旋转角度
        else
        {
            agent.SetDestination(currentDestination);//巡逻时向目标移动
            animator.SetBool("Walk", true);//行走动画
            if (Vector3.Distance(transform.position, currentDestination) <= 0.5f)
            {
                //animator.SetBool("Walk", false);
                ChangeRoad();
            }
            /* if (Time.time - lastTime >= playerThinkTime)
             {
                 lastTime = Time.time;

                 int rand = Random.Range(0, 2);
                 switch (rand)
                 {
                     case 0:

                         this.GetComponent<Animation>().Play("Idle");
                         playerState = stateIdle;
                         break;
                     case 1:
                         Quaternion rotate = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
                         m_transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 1000);//一秒内完成旋转
                         animator.SetBool("Walk", true);
                         //this.GetComponent<Animation>().Play("walk");//行走动画
                         //animator.SetTrigger("Walk");
                         m_transform.Translate(Vector3.forward * Time.deltaTime);
                         playerState = stateWalk;
                         break;

                     case 2:
                         //animator.SetBool("Walk", false);
                         //animator.SetBool("Idle", false);
                        // animator.SetBool("Run", true);
                         this.GetComponent<Animation>().Play("run");
                         m_transform.Translate(Vector3.forward * Time.deltaTime);
                         playerState = stateRun;
                         break;
                 }

             }*/
        }
    }
 /*   void InitWayPoint()
    {
        GameObject obj = GameObject.Find("WaypointContainer");
        if (obj)
            path.InitByObj(obj);
    }
    */
/*	void Control()
	{
		//定义三个移动值；
		float xm =0,ym=0,zm=0;
		//重力运动
		ym -= m_gravity * Time.deltaTime;
		//上下左右运动
		if(Input.GetKey(KeyCode.W))
		{
			zm+=m_movSpeed*Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S))
		{
			zm-=m_movSpeed*Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.A))
		{
			xm-=m_movSpeed*Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D))
		{
			xm+=m_movSpeed*Time.deltaTime;
		}
		m_ch.Move(m_transform.TransformDirection(new Vector3(xm,ym,zm)));
	}*/
}
