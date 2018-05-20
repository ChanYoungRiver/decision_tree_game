using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AIBehaviorTree
{




	//巡逻
	public class ActionSoliderMoveRound : BNodeAction
	{
		private float m_ftime;

		public ActionSoliderMoveRound()
			:base()
		{
//			Debug.Log("ActionSoliderMoveRound");
			this.m_strName = "ActionSoliderMoveRound";
		}

		public override void OnEnter (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			tinput.MoveAround ();
//			Debug.Log("MoveAround");
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			if ( tinput.NearbyExistEnemy ()) {
				return ActionResult.FAILURE;
			}else if (tinput.moveRoundTweener==null ) {
				return ActionResult.SUCCESS;
			} else {
				return ActionResult.RUNNING;
			}
				

		}
	}

	//攻击敌人
	public class ActionSoliderAttack : BNodeAction
	{
		private float m_ftime;
		public bool over{ get; set;}

		public ActionSoliderAttack()
			:base()
		{
			this.m_strName = "ActionSoliderAttack";
			this.over = true;
		}

		public override void OnEnter (BInput input)
		{
			this.over = false;
			SoliderPlayer tinput = input as SoliderPlayer;
			tinput.AttackEnemy (this);
			this.m_ftime = Time.time;
//			Debug.Log("ActionSoliderAttack");
		}

		//excute
		public override ActionResult Excute (BInput input)
		{

			if(Time.time - this.m_ftime > 0.5f)
				this.over = true;
			if (this.over) {
//				Debug.Log("ActionSoliderAttack SUCCESS");
				return ActionResult.SUCCESS;
			}
				
//			Debug.Log("ActionSoliderAttack RUNNING");
			return ActionResult.RUNNING;


		}
	}

	//范围内是否存在敌人
	public class ConditionNearbyAliveEnemy : BNodeCondition
	{
		public ConditionNearbyAliveEnemy()
			:base()
		{
//			Debug.LogError ("ConditionNearbyAliveEnemy");
			this.m_strName = "ConditionNearbyAliveEnemy";
		}


		public override ActionResult Excute (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			bool isEnemyInSight = tinput.NearbyExistEnemy();
			if(isEnemyInSight)
			{
//				Debug.LogError ("ConditionNearbyAliveEnemy 范围内存在敌人"+this.m_strName);
				return ActionResult.SUCCESS;
			}
//			Debug.LogError ("ConditionNearbyAliveEnemy 范围内不存在敌人"+this.m_strName);
			return ActionResult.FAILURE;
		}
	}

	//范围内不存在敌人
	public class ConditionNearbyNoEnemy : BNodeCondition
	{
		public ConditionNearbyNoEnemy()
			:base()
		{
//			Debug.LogError ("ConditionNearbyNoEnemy");
			this.m_strName = "ConditionNearbyNoEnemy";
		}


		public override ActionResult Excute (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			bool isEnemyInSight = tinput.NearbyExistEnemy();
			if(!isEnemyInSight)
			{
//				Debug.LogError ("ConditionNearbyNoEnemy 范围内不存在敌人"+this.m_strName);
				return ActionResult.SUCCESS;
			}
//			Debug.LogError ("ConditionNearbyNoEnemy 范围内存在敌人"+this.m_strName);
			return ActionResult.FAILURE;
		}
	}

	//全场是否存在敌人
	public class ConditionAliveEnemy : BNodeCondition
	{
		public int HP;

		public ConditionAliveEnemy()
			:base()
		{
			this.m_strName = "ConditionAliveEnemy";
		}

		public override ActionResult Excute (BInput input)
		{
			bool isExist = GameLauncher.GetInstance().CheckExistEnemy();
			if(isExist)
			{
//				Debug.LogError ("ConditionAliveEnemy 存在敌人");
				return ActionResult.SUCCESS;
			}
//			Debug.LogError ("ConditionAliveEnemy 不存在敌人");
			return ActionResult.FAILURE;
		}
	}

	//全场不存在敌人
	public class ConditionNoAliveEnemy : BNodeCondition
	{
		public int HP;

		public ConditionNoAliveEnemy()
			:base()
		{
			this.m_strName = "ConditionNoAliveEnemy";
		}

		public override ActionResult Excute (BInput input)
		{
			bool isExist = GameLauncher.GetInstance().CheckExistEnemy();
			if(!isExist)
			{
//				Debug.LogError ("ConditionNoAliveEnemy 存在敌人");
				return ActionResult.SUCCESS;
			}
//			Debug.LogError ("ConditionNoAliveEnemy 不存在敌人");
			return ActionResult.FAILURE;
		}
	}

	//停止状态
	public class ActionSoliderStop : BNodeAction
	{

		public ActionSoliderStop()
			:base()
		{
			this.m_strName = "ActionSoliderStop";
		}

		public override void OnEnter (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			tinput.StopAllAction();
//			Debug.Log("ActionSoliderStop");
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			return ActionResult.SUCCESS;
	
		}
	}

	//--返回基地
	public class ActionSoliderGoHome:BNodeAction
	{

		public ActionSoliderGoHome()
			:base()
		{
			this.m_strName = "ActionSoliderGoHome";
		}

		public override void OnEnter (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			tinput.GoHome ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			return ActionResult.SUCCESS;

		}
	}

	//敌人逃跑
	public class ActionEnemySoliderEscape:BNodeAction
	{
		public ActionEnemySoliderEscape()
			:base()
		{
			this.m_strName = "ActionEnemySoliderEscape";
		}

		public override void OnEnter (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			tinput.Escape ();
		}
			
		public override ActionResult Excute (BInput input)
		{
			return ActionResult.SUCCESS;

		}		
	}

	//范围内是否存在敌人 通用版
	public class ConditionCommonNearbyAliveEnemy : BNodeCondition
	{
		public ConditionCommonNearbyAliveEnemy()
			:base()
		{
			this.m_strName = "ConditionCommonNearbyAliveEnemy";
		}


		public override ActionResult Excute (BInput input)
		{
			SoliderPlayer tinput = input as SoliderPlayer;
			bool isEnemyInSight = tinput.NearbyExistEnemy();
			if(isEnemyInSight)
			{
								Debug.LogError ("ConditionNearbyAliveEnemy 范围内存在敌人"+this.m_strName);
				return ActionResult.SUCCESS;
			}
						Debug.LogError ("ConditionNearbyAliveEnemy 范围内不存在敌人"+this.m_strName);
			return ActionResult.FAILURE;
		}
	}


}