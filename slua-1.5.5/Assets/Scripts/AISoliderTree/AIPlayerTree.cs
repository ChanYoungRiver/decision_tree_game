using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.AIBehaviorTree
{
	//是否存活
	public class ConditionAIISAlive : BNodeCondition
	{

		public ConditionAIISAlive()
			:base()
		{
			this.m_strName = "ConditionAIISAlive";
		}
			
		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			Debug.Log ("ConditionAIISAlive"+tinput.hp);
			if ( tinput.hp>0) {
				return ActionResult.SUCCESS;
			}else {
				return ActionResult.FAILURE;
			}


		}
	}

	//执行死亡
	public class ActionAIDie : BNodeAction
	{

		public ActionAIDie()
			:base()
		{
			this.m_strName = "ActionAIDie";
		}

		public override void OnEnter (BInput input)
		{
			Debug.Log ("ActionAIDie");
			AIPlayer tinput = input as AIPlayer;
			tinput.Die ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if ( tinput.hp>0) {
				return ActionResult.FAILURE;
			}else if (tinput.isDieActionRun==false ) {
				return ActionResult.SUCCESS;
			} else {
				return ActionResult.RUNNING;
			}


		}
	}

	//是否危急
	public class ConditionAINotEnoughHP : BNodeCondition
	{
		public int HP;
		public ConditionAINotEnoughHP()
			:base()
		{
			this.m_strName = "ConditionAINotEnoughHP";
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			Debug.Log ("ConditionAINotEnoughHP"+tinput.hp+" "+HP);
			if ( tinput.hp<=HP) {
				return ActionResult.SUCCESS;
			}else {
				return ActionResult.FAILURE;
			}
		}
	}

	//是否已经到目的地，停止移动
	public class ConditionAIHasMoveToTarget : BNodeCondition
	{
		public int HP;
		public ConditionAIHasMoveToTarget()
			:base()
		{
			this.m_strName = "ConditionAIHasMoveToTarget";
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if (!tinput.isMove) {
				Debug.Log ("ConditionAIHasMoveToTarget SUCCESS");
				return ActionResult.SUCCESS;
			}else {
				Debug.Log ("ConditionAIHasMoveToTarget FAILURE");
				return ActionResult.FAILURE;
			}
		}
	}


	//执行移动
	public class ActionAIMoveToTarget : BNodeAction
	{

		public ActionAIMoveToTarget()
			:base()
		{
			this.m_strName = "ActionAIMoveToTarget";
		}

		public override void OnEnter (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			tinput.MoveToTarget ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			 if (tinput.isMove==false ) {
				return ActionResult.SUCCESS;
			} else {
				return ActionResult.RUNNING;
			}
		}
	}

	//随机移动
	public class ActionAIRandomMove : BNodeAction
	{

		public ActionAIRandomMove()
			:base()
		{
			this.m_strName = "ActionAIRandomMove";
		}

		public override void OnEnter (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			Debug.Log ("ActionAIRandomMove");
			tinput.RandomMove ();
		}

		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if (tinput.isMove==false ) {
				return ActionResult.SUCCESS;
			} else {
				return ActionResult.RUNNING;
			}
		}
	}

	//执行回家
	public class ActionAIMoveToHome : ActionAIMoveToTarget
	{

		public ActionAIMoveToHome()
			:base()
		{
			this.m_strName = "ActionAIMoveToHome";
		}

		public override void OnEnter (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			tinput.MoveBackHome ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			Debug.Log ("ActionAIMoveToHome");
			AIPlayer tinput = input as AIPlayer;
			if ( tinput.IsBackHome()) {
				return ActionResult.FAILURE;
			}else if (tinput.isMoveToHome==false ) {
				return ActionResult.SUCCESS;
			} else {
				return ActionResult.RUNNING;
			}


		}
	}

	//视野内是否存在敌人
	public class ConditionAIHasEnemyInView : BNodeCondition
	{
		public int HP;
		public ConditionAIHasEnemyInView()
			:base()
		{
			this.m_strName = "ConditionAIHasEnemyInView";
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			
			AIPlayer tinput = input as AIPlayer;
			if ( tinput.isEnemyInView()) {
				Debug.Log ("ConditionAIHasEnemyInView SUCCESS");
				return ActionResult.SUCCESS;
			}else {
				Debug.Log ("ConditionAIHasEnemyInView FAILURE");
				return ActionResult.FAILURE;
			}
		}
	}

	//是否拥有子弹
	public class ConditionAIHasAmmo : BNodeCondition
	{
		public int HP;
		public ConditionAIHasAmmo()
			:base()
		{
			this.m_strName = "ConditionAIHasAmmo";
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if ( tinput.ammoNum>0) {
				return ActionResult.SUCCESS;
			}else {
				return ActionResult.FAILURE;
			}
		}
	}

	//填装子弹
	public class ActionAIAddAmmo : ActionAIMoveToTarget
	{
		public bool over { get; set; }
		private float m_ftime;
		public ActionAIAddAmmo()
			:base()
		{
			this.m_strName = "ActionAIAddAmmo";
		}

		public override void OnEnter (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			this.m_ftime = Time.time;
			this.over = false;
			Debug.LogError ("装填子弹OnEnter");
			tinput.AddAmmo ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if(Time.time - this.m_ftime > tinput.loadAmmoTime)
				this.over = true;
			if(this.over)
				return ActionResult.SUCCESS;
			return ActionResult.RUNNING;

		}
	}

	//敌人是否在射击范围内
	public class ConditionAICanShoot : BNodeCondition
	{
		public int HP;
		public ConditionAICanShoot()
			:base()
		{
			this.m_strName = "ConditionAICanShoot";
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if ( tinput.isEnemyInShootDistance()) {
				return ActionResult.SUCCESS;
			}else {
				return ActionResult.FAILURE;
			}
		}
	}

	//执行射击
	public class ActionAIShoot : BNodeAction
	{
		public bool over { get; set; }
		private float m_ftime;
		public ActionAIShoot()
			:base()
		{
			this.m_strName = "ActionAIShoot";
		}

		public override void OnEnter (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			this.m_ftime = Time.time;
			this.over = false;
			tinput.Shoot ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if(Time.time - this.m_ftime > tinput.shootSpace)
				this.over = true;
			if(this.over)
				return ActionResult.SUCCESS;
			return ActionResult.RUNNING;
		}
	}

	//执行追击
	public class ActionAIPursue : BNodeAction
	{
		public ActionAIPursue()
			:base()
		{
			this.m_strName = "ActionAIPursue";
		}

		public override void OnEnter (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			tinput.Pursue ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if(!tinput.isMove)
				return ActionResult.SUCCESS;
			return ActionResult.RUNNING;
		}
	}

	//50%随机移动或者休息
	public class ConditionAIRandomChance:BNodeCondition
	{

		public int HP;
		public ConditionAIRandomChance()
			:base()
		{
			this.m_strName = "ConditionAIRandomChance";
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			int value = Random.Range (0, 100);
			if ( value>90) {
				Debug.Log ("ConditionAIRandomChance SUCCESS");
				return ActionResult.SUCCESS;
			}else {
				Debug.Log ("ConditionAIRandomChance FAILURE");
				return ActionResult.FAILURE;
			}
		}
		
	}

	//执行休息
	public class ActionAIIdle : BNodeAction
	{
		public bool over { get; set; }
		private float m_ftime;
		public ActionAIIdle()
			:base()
		{
			this.m_strName = "ActionAIIdle";
			this.over = true;
		}

		public override void OnEnter (BInput input)
		{
			Debug.Log ("ActionAIIdle");
			AIPlayer tinput = input as AIPlayer;
			this.m_ftime = Time.time;
			this.over = false;
			tinput.rest ();
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
			AIPlayer tinput = input as AIPlayer;
			if(Time.time - this.m_ftime > tinput.resetTime||this.over==true)
				this.over = true;
			
			if (this.over) {
				return ActionResult.SUCCESS;
			} else {
				return ActionResult.RUNNING;
			}
				


		}
	}

}


