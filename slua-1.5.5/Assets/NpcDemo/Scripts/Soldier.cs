using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Soldier : MonoBehaviour {
    public enum SOLDIER_STATE { PATROL, CHASE, ATTACK, SHOOT };
    public SOLDIER_STATE CurrenrState
    {
        get { return currentstate; }
        set
        {
            currentstate = value;
            StopAllCoroutines();
            switch (currentstate)
            {
                case SOLDIER_STATE.PATROL:
                    StartCoroutine(AIPatrol());
                    break;
                case SOLDIER_STATE.CHASE:
                    StartCoroutine(AIChase());
                    break;
                case SOLDIER_STATE.ATTACK:
                    StartCoroutine(AIAttack());
                    break;
            }
        }
    }
    [SerializeField]
    private SOLDIER_STATE currentstate = SOLDIER_STATE.PATROL;
    private LineSight ThisLineSight = null;
    private NavMeshAgent ThisAgent = null;
    private Transform EnemyTransform = null;
    private Transform PatrolDestination = null;
    //private Health PlayerHealth = null;
    private Animator animator = null;
    public float MaxDamege = 10f;
    void Awake()
    {
        ThisLineSight = GetComponent<LineSight>();
        ThisAgent = GetComponent<NavMeshAgent>();//导航代理
        EnemyTransform = GameObject.FindGameObjectWithTag("enemy").GetComponent<Transform>();
        
        animator = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {       
        GameObject[] Destinations = GameObject.FindGameObjectsWithTag("Dest");
        PatrolDestination = Destinations[Random.Range(0, Destinations.Length)].GetComponent<Transform>();
        CurrenrState = SOLDIER_STATE.PATROL;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public IEnumerator AIPatrol()
    {
        while (currentstate == SOLDIER_STATE.PATROL)
        {
            print("我已经打到你了！" + Random.value);
            ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(PatrolDestination.position);
            while (ThisAgent.pathPending)
                yield return null;
            if (ThisLineSight.CanSeeTarget)
            {
                ThisAgent.isStopped = true;
                CurrenrState = SOLDIER_STATE.CHASE;
                yield break;
            }
        }
        yield return null;
    }
    public IEnumerator AIChase()
    {
        while (currentstate == SOLDIER_STATE.CHASE)
        {
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(ThisLineSight.LastKnowSighting);
            while (ThisAgent.pathPending)
                yield return null;
            if(ThisAgent.remainingDistance<=ThisAgent.stoppingDistance)
            {
                ThisAgent.isStopped = true;

                if (!ThisLineSight.CanSeeTarget)
                {
                    CurrenrState = SOLDIER_STATE.PATROL;
                }
                else
                {
                    CurrenrState = SOLDIER_STATE.ATTACK;
                }
                yield break;
            }
            
        }
        yield return null;
    }
    public IEnumerator AIAttack()
    {
        while (currentstate == SOLDIER_STATE.ATTACK)
        {
            ThisAgent.isStopped = false;

            ThisAgent.SetDestination(ThisLineSight.LastKnowSighting);
            while (ThisAgent.pathPending)
                yield return null;
            if (ThisAgent.remainingDistance > ThisAgent.stoppingDistance)
            {
                CurrenrState = SOLDIER_STATE.CHASE;
                yield break;
            }
            else
            {

            }

            yield return null;
        }
        yield return null;
    }

}
