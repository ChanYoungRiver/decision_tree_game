using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletScript3D : MonoBehaviour {

	private float speed = 20.0f;
	public int damage = 20;//伤害值
	public Tweener tweener = null;//动画
	public bool isFinish = false;
	public AIPlayerGroup bulleGroup;//子弹主人
	// Use this for initialization
	void Start () {
		isFinish = false;
	}

	// Update is called once per frame
	void Update () {

	}

	public void RuningBullet(Vector3 origial,Vector3 target){

//		Vector3 dir =(target.position - origial.position);

//		Vector3 target_ = target.position;
//		Debug.LogError("RuningBullet "+origial+" "+target);
		//3.飞行距离
		float dis = (origial-target).magnitude ;//计算两点间的距离
		if (tweener!=null){
			tweener.Kill ();//结束动画
		}
		tweener = this.GetComponent<Transform> ().DOMove( target  , dis/speed );//时间
		tweener.SetEase (Ease.Linear);//设置类型
	}

	void OnTriggerEnter(Collider collider3D){
		//		Debug.Log ("BulletScript1"+collider2D.gameObject.name); 

		if (collider3D.transform.CompareTag ("EnemyPhy")) {
			AI3DPlayer enemyPlayer = collider3D.transform.parent.GetComponent<AI3DPlayer> ();
			if (enemyPlayer.belongGroup!=bulleGroup){
				isFinish = true;
				if (tweener!=null){
					tweener.Kill ();
					tweener = null;
					this.gameObject.SetActive (false);
				}
			}
		}

	}
}
