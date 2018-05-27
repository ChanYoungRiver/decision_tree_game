using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GameManager : MonoBehaviour {
    private Animator animator;
    private NavMeshAgent agent;
    private LineSight ThisLineSight;
    private Enemy ThisEnemy;//敌人的类
    private GameObject enemy;//敌人

    private int playerState;//记录士兵当前状态

    public int lifeValue = 999999;//生命值
    public int ammoNum = 6;//弹药数量
    public const int stateIdle = 0;//休息状态
    public const int stateWalk = 1;//行走状态
    public const int stateRun = 2;//奔跑状态
    public const int stateShoot = 3;//射击状态
    public const int stateAttack = 4;//徒手攻击状态

    public const float runDistance = 50.0f;//可追逐范围
    public const float shookDistance = 25.0f;//可射击范围
    public const float attackDistance = 1.5f;//可徒手攻击范围
    public Transform m_transform;//士兵的transform
    public LayerMask layer; // 射击时射线能射到的碰撞层  
    public Transform fx; // 射中目标后的粒子效果  
    public Rigidbody Bullet;
    public Transform muzzlePoint; // 枪口的Transform组件  
    private float shootTimer = 0; // 射击间隔的计时器 
    Text lab_ammoNum;//弹药数量
    Text lab_lifeValue;//生命值
    CharacterController m_ch;//角色控制器组件

    float m_movSpeed = 0.5f;//角色移动速度
    float m_gravity = 2.0f;//重力
    float ammoSpeed = 1f;//子弹速度
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
        playerState = stateIdle;
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();//导航代理
        enemy = GameObject.FindGameObjectWithTag("enemy");
        muzzlePoint = GameObject.Find("MP40GermanmachineGun").GetComponent<Transform>();
        lab_ammoNum = GameObject.Find("lab_ammoNum").GetComponent<Text>();
        lab_lifeValue = GameObject.Find("lab_lifeValue").GetComponent<Text>();
        originalPosition = transform.position;   //士兵出生点位置（视为基地)  
        aPosition = new Vector3(-28, originalPosition.y, -37); //巡逻位置A  
        bPosition = new Vector3(34, originalPosition.y, 32); //巡逻位置B  
        currentDestination = aPosition;
    }
    // Update is called once per frame
    void Update()
    {
        //如果生命为0，什么也不做
        if (lifeValue <= 0)
            return;
        if (Input.GetKeyDown(KeyCode.J))
        {

            Rigidbody clone;

            clone = (Rigidbody)Instantiate(Bullet, muzzlePoint.position, muzzlePoint.rotation);

            clone.velocity = transform.TransformDirection(Vector3.forward * ammoSpeed);

        }
        AttackDistance();//判断敌人
    }
    public void ChangeRoad()
    {

        if (currentDestination == aPosition)
        {
            currentDestination = bPosition;
        }
        else
        {
            currentDestination = aPosition;
        }
    }
    public void SetAmmo(int ammo)
    {
        ammoNum = ammoNum - ammo;
        //如果弹药为负数。重新填充
        if (ammoNum <= 0)
        {
            ammoNum = 6 - ammoNum;
        }
        lab_ammoNum.text = ammoNum.ToString() + "/6";
    }
    //更新生命
    public void SetLife(int life)
    {
        lab_lifeValue.text = life.ToString();
    }
    //判断敌人与主角的距离
    void AttackDistance()
    {
        //ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;
        if (ThisLineSight.CanSeeTarget || Vector3.Distance(m_transform.position, enemy.transform.position) <= 5.0f)
        {

            agent.isStopped = true;
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;
            // agent.isStopped = false;
            m_transform.LookAt(enemy.transform);//面朝敌人
                                                //  m_transform.Translate(Vector3.forward * Time.deltaTime);
            agent.SetDestination(ThisLineSight.LastKnowSighting);
            // animator.Play("Attack");

            print("我已经打到你了！" + Random.value);
            animator.SetBool("Attack", true);
            //ThisEnemy.OnDamage(1);
            //animator.SetBool("Run", false);
        }
        /*  if (Vector3.Distance(transform.position, enemy.transform.position) <= runDistance)
          {
              animator.SetBool("Run", true);
              //animator.SetBool("Shoot", false);
             // animator.SetBool("Attack", false);
              if (Vector3.Distance(transform.position, enemy.transform.position) <= attackDistance)
              {
                  animator.SetBool("Attack", true);
                 // animator.SetBool("Run", false);
                 // animator.SetBool("Walk", false);
                  playerState = stateAttack;//当前士处于射击状态
                  ShootAction();
                  print("我已经打到你了！" + Random.value);
                  //Vector3 direction = enemy.transform.position - transform.position;
                 if (Vector3.Distance(transform.position, enemy.transform.position) <= shookDistance)
                  {
                      animator.SetBool("Shoot", true);
                      animator.SetBool("Run", false);
                      animator.SetBool("Shoot", false);
                      animator.SetBool("Walk", false);
                      playerState = stateAttack;//当前士处于射击状态
                      print("我已经打到你了！" + Random.value);
                  }

              }           
              playerState = stateRun;
              m_transform.LookAt(enemy.transform);//面朝敌人
              m_transform.Translate(Vector3.forward * Time.deltaTime);

          }*/
        //随机旋转角度
        else
        {
            //ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;
            agent.SetDestination(currentDestination);//巡逻时向目标移动
                                                     //animator.SetBool("Attack", true);//行走动画
                                                     //animator.SetBool("Run", false);
                                                     // animator.Play("Attack", 10);
                                                     //animator.Play("Walk");
            animator.SetBool("Walk", true);
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
    /// 主角被攻击时触发的逻辑代码  
    /// </summary>  
    /// <param name="damage">伤害数值</param>  
    public void OnDamage(int damage)
    {
        lifeValue -= damage;
        // 更新UI生命值  
        SetLife(lifeValue);
        if (lifeValue <= 400)
        {

            //animator.SetBool("Shoot", true);
            ShootAction();
            // 如果生命小于200则逃走
            if (lifeValue <= 200)
            {
                agent.isStopped = false;
                //animator.Play("Run");
                animator.SetBool("Run", true);
                //animator.SetBool("Walk", false);
                // animator.SetBool("attack", false);
                //animator.SetBool("Idle", false);

                agent.SetDestination(originalPosition);
                if (Vector3.Distance(m_transform.position, originalPosition) <= 0.5f)
                {
                    animator.Play("Idle");

                    //animator.SetBool("Idle", true);
                    SetLife(999999);//回复生命
                }
            }
        }
    }
    //射击动作，射出一条射线来模拟射击功能，如果射线与敌人相碰撞则使敌人减少一定生命值
    public void ShootAction()
    {
        // 更新射击间隔时间  
        shootTimer -= Time.deltaTime;
        // 如果射击  
        if (playerState == stateShoot && shootTimer <= 0)
        {
            shootTimer = 0.1f;
            animator.Play("Shoot");
            // 更新UI，减少弹药数量  
            SetAmmo(1);
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

}
