using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    Transform m_transform;
    Animator m_ani;
    Player m_player;
    NavMeshAgent m_agent;
    float m_moveSpeed = 2.5f;
    float m_rotSpeed = 30;
    //计时器
    float m_timer = 2;
    int m_life=15;
	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        //获取动画组件
        m_ani = GetComponent<Animator>();
        //获得警官
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_agent = GetComponent<NavMeshAgent>();
        //指定寻路器的行走速度
        m_agent.speed = m_moveSpeed;
        //设置寻路目标
        m_agent.SetDestination(m_player.m_transform.position);
	}
	void RotateTo()
    {//获取目标方向
        Vector3 targetdir = m_player.m_transform.position - m_transform.position;
        //计算出新的方向
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);//通过目标点的位置和自身的位置，计算出转向目标点的角度
        //旋转至新方向
        m_transform.rotation = Quaternion.LookRotation(newDir);
    }
    protected bool AgentStopping()
    {
        return m_agent.remainingDistance <= m_agent.stoppingDistance;
    }
	// Update is called once per frame
	void Update () {
        if (m_player.m_life <= 0)
            return;
        //获取当前动画状态
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        //如果处于待机状态
        if(stateInfo.shortNameHash == Animator.StringToHash("idle")&& !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);
            m_timer -= Time.deltaTime;
            //待机一定时间
            if (m_timer > 0)
                return;
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <1.5f)
            {
                m_ani.SetBool("attack", true);
            }
            else
            {
                m_timer = 1;
                m_agent.SetDestination(m_player.m_transform.position);
                m_ani.SetBool("run", true);
            }
        }
        if (stateInfo.shortNameHash == Animator.StringToHash("run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);
            //每隔一秒重新定位主角的位置
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <= 1.5f)
            {
                //m_agent.Stop();
                m_ani.SetBool("attack", true);
            }
        }
        if (stateInfo.shortNameHash == Animator.StringToHash("attack") && !m_ani.IsInTransition(0))
        {
            RotateTo();
            m_ani.SetBool("attack", false);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);
                m_timer = 2;
            }
        }
    }
}
