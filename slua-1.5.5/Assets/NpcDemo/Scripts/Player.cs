using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour {

    public int ammoNum = 20;//子弹数量
    public int health = 999;//生命值
    public Transform m_transform;//士兵的transform
    public Transform muzzlePoint; // 枪口的Transform组件
    public LayerMask layer; // 射击时射线能射到的碰撞层  
    public Transform fx; // 射中目标后的粒子效果
    private Animator animator;//动画组件
    private NavMeshAgent agent;//寻路组件
    private LineSight ThisLineSight;//视野
    private bool alive;//是否有生命
    private GameObject enemy;//敌人
    private float shootTimer = 0.9f;//射击间隔时间器
    Text lab_ammoNum;//弹药数量UI
    Text lab_lifeValue;//生命值UI
    Vector3 originalPosition;//士兵初始位置
    Vector3 aPosition = Vector3.zero;//巡逻点A的位置
    Vector3 bPosition = Vector3.zero;//巡逻点B的位置
    Vector3 currentDestination = Vector3.zero;//士兵现在要巡逻的位置
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        ThisLineSight = GetComponent<LineSight>();
    }
    // Use this for initialization
    void Start()
    {
        m_transform = this.transform;
        agent = GetComponent<NavMeshAgent>();//导航代理
        enemy = GameObject.FindGameObjectWithTag("enemy");
        muzzlePoint = GameObject.Find("MP40GermanmachineGun").GetComponent<Transform>();//枪
        lab_ammoNum = GameObject.Find("lab_ammoNum").GetComponent<Text>();//子弹UI
        lab_lifeValue = GameObject.Find("lab_lifeValue").GetComponent<Text>();//生命UI
        originalPosition = m_transform.position;   //士兵出生点位置（视为基地)  
        aPosition = new Vector3(-28, originalPosition.y, -37); //巡逻位置A  
        bPosition = new Vector3(34, originalPosition.y, 32); //巡逻位置B  
        currentDestination = bPosition;
    }
    // Update is called once per frame
    void Update()
    {
        //如果生命为0，什么也不做
        if (health <= 0)
            Destroy(gameObject);
        //AIPatrol();

        AIShoot();
    }
    //巡逻动作
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
    //追逐动作
    public void AIPursue()
    {
        agent.SetDestination(enemy.transform.position);
        animator.SetBool("Run", true);
    }
    //逃跑动作
    public void AIFlee()
    {
        agent.SetDestination(originalPosition);//跑回到基地
        animator.SetBool("Run", true);
        if (Vector3.Distance(transform.position, originalPosition) <= 1.5f)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", true);
            SetLife(999);//生命重置
            agent.isStopped = false;
        }
    }
    //射击动作
    public void AIShoot()
    {
        //animator.SetBool("Shoot", true);
        animator.Play("Shoot");
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {            
            SetAmmo(1);
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
            Instantiate(fx, info.point, info.transform.rotation);
        }
    }
    //死亡判断
    public bool IsDeath()
    {
        if (health <= 0)
        {
            alive = false;
            animator.Play("Death");
            Destroy(gameObject);
        }
        else
            alive = true;
        return alive;
    }
    public void OnDamage(int damage)
    {
        health -= damage;
        // 更新UI生命值  
        SetLife(health);
    }
        //更新生命UI
        public void SetLife(int life)
    {
        lab_lifeValue.text = life.ToString();
    }
    //装弹函数
    public void SetAmmo(int ammo)
    {
        ammoNum = ammoNum - ammo;
        //如果弹药为负数,重新填充
        if (ammoNum <= 0)
        {
            ammoNum = 20 - ammoNum;
        }
        lab_ammoNum.text = ammoNum.ToString() + "/20";
    }
}
