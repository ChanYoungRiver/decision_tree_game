using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    Transform m_transform;
    Animator m_ani;//动画组件
    Player m_player;//士兵
    NavMeshAgent m_agent;
    //float m_moveSpeed = 0.5f;
    float m_rotSpeed = 30;  //敌人旋转速度
    float m_timer = 2;//计时器
    int m_life=1000;//敌人生命值
    Vector3 originalPosition;//士兵初始位置
    protected EnemySpawn m_spawn;
    // Use this for initialization
    void Start () {
        m_transform = this.transform;
        originalPosition = transform.position;
        m_ani = GetComponent<Animator>();//获取动画组件

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//获得士兵
        m_agent = GetComponent<NavMeshAgent>();

        //m_agent.speed = m_moveSpeed;//指定寻路器的行走速度
        //设置寻路目标
       
	}
    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.enemyCount++;
    }
    /// <summary>  
    /// 敌人被击中时触发的逻辑代码  
    /// </summary>  
    /// <param name="damage">伤害数值</param>  
    public void OnDamage(int damage)
    {
        m_life -= damage;
        // 如果生命为0，进入死亡状态 
        if (m_life <=300)
        {
            m_ani.SetBool("run", true);
            m_ani.SetBool("attack", false);
            m_agent.SetDestination(originalPosition);
        }
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
        }
    }
    //转向目标点
    void RotateTo()
    {
        //获取目标方向
        Vector3 targetdir = m_player.transform.position - m_transform.position;
        //计算出新的方向
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);//通过目标点的位置和自身的位置，计算出转向目标点的角度
        //旋转至新方向
        m_transform.rotation = Quaternion.LookRotation(newDir);
    }
	// Update is called once per frame
	void Update () {
        if (m_player.lifeValue <= 0)
            return;
        m_agent.SetDestination(m_player.transform.position);
        if (Vector3.Distance(m_transform.position, m_player.transform.position) < 1.5f)
        {
            m_ani.SetBool("attack", true);
            m_player.OnDamage(1);
        }
        EnemyState();
 
    }
    void EnemyState()
    { //获取当前动画状态
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        // 死亡状态  
        if (stateInfo.fullPathHash == Animator.StringToHash("death") && !m_ani.IsInTransition(0))
        {
            // 判断死亡动画是否播放完成  
            if (stateInfo.normalizedTime >= 1.0f)
            {
                // 销毁自身  
                Destroy(gameObject);
            }
        }
        //如果处于待机状态
        if (stateInfo.shortNameHash == Animator.StringToHash("idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);
            m_timer -= Time.deltaTime;
            //待机一定时间
            if (m_timer > 0)
                return;
            if (Vector3.Distance(m_transform.position, m_player.transform.position) < 1.5f)
            {
                m_ani.SetBool("attack", true);
            }
            else
            {
                m_timer = 1;
                // m_agent.speed = 0.5f; m_agent.SetDestination(m_player.transform.position);
                m_agent.SetDestination(m_player.transform.position);
                m_ani.SetBool("run", true);
            }
        }
        //奔跑状态
        if (stateInfo.shortNameHash == Animator.StringToHash("run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);
            //每隔一秒重新定位主角的位置
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.transform.position);
                m_timer = 1;
            }
            if (Vector3.Distance(m_transform.position, m_player.transform.position) <= 1.5f)
            {
                m_agent.isStopped = true;
                m_ani.SetBool("attack", true);
            }
        }
        // 攻击状态  
        if (stateInfo.shortNameHash == Animator.StringToHash("attack") && !m_ani.IsInTransition(0))
        {
            RotateTo();
            m_ani.SetBool("attack", false);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);
                m_timer = 2;
                m_player.OnDamage(1);
            }
        }
        if(stateInfo.shortNameHash == Animator.StringToHash("death") && !m_ani.IsInTransition(0))
        {
            if(stateInfo.normalizedTime>=1.0f)
            {
                m_spawn.enemyCount--;
                Destroy(this.gameObject);
            }
        }
    }
}
