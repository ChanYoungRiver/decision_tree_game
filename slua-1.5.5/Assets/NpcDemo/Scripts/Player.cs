using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour {
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject enemy;//敌人
    private int playerState;//记录士兵当前状态
    public const int stateIdle = 0;//休息状态
    public const int stateWalk = 1;//行走状态
    public const int stateRun = 2;//奔跑状态

    public const float attackDistance = 10.0f;//巡逻范围
    public Transform m_transform;
    public Transform taregt;//移动到目标对象


    Vector3 destination;
    //角色控制器组件
    CharacterController m_ch;
    //角色移动速度
    float m_movSpeed = 3.0f;
    //重力
    float m_gravity = 2.0f;
    //生命值
    public int m_life = 5;
    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start() {
        playerState = stateIdle;
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.Find("Enemy");
    }

    // Update is called once per frame
    void Update() {
        //如果生命为0，什么也不做
        if (m_life <= 0)
            return;
        AttackDistance();//判断敌人距离
                         //m_transform.Translate(Vector3.forward * Time.deltaTime);
                         // agent.destination = taregt.position;
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
    //判断敌人与主角的距离
    void AttackDistance()
    {
        if (Vector3.Distance(transform.position, enemy.transform.position) < attackDistance)
        {
            animator.SetBool("Run", true);
            playerState = stateRun;
            transform.LookAt(enemy.transform);//面朝敌人
        }
        //随机旋转角度
        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) { animator.SetBool("Idle", true); playerState = stateIdle; }
            else if (rand == 1)
            {
                Quaternion rotate = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 1000);//一秒内完成旋转
                animator.SetBool("Walk", true);//行走动画
                playerState = stateWalk;
            }
        }
        switch (playerState)
        {
            case stateIdle:
                break;
            case stateWalk:
                transform.Translate(Vector3.forward * Time.deltaTime);
                break;
            case stateRun:
                if (Vector3.Distance(transform.position, enemy.transform.position) > 3.0f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * 3);
                }
                break;
        }
    }
 
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
