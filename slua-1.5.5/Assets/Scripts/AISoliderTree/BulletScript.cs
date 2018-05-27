using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletScript : MonoBehaviour {

	private float speed = 5.0f;
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

	public void RuningBullet(Transform origial,Transform target){

		Vector2 dir = (Vector2)(target.position - origial.position);
		//2.子弹方向
		dir = new Vector2( dir.x*100,dir.y*100  );
		Vector2 target_ = dir + (Vector2)origial.position;

		//3.飞行距离
		float dis = (origial.position-target.position).magnitude ;//计算两点间的距离
		if (tweener!=null){
			tweener.Kill ();//结束动画
		}
		tweener = this.GetComponent<RectTransform> ().DOMove( new Vector3(target_.x,target_.y,0)  , dis/speed );//时间
		tweener.SetEase (Ease.Linear);//设置类型
	}

	void OnTriggerEnter2D(Collider2D collider2D){
//		Debug.Log ("BulletScript1"+collider2D.gameObject.name); 
		AIPlayer enemyPlayer = collider2D.GetComponent<AIPlayer> ();
		if (enemyPlayer != null && enemyPlayer.belongGroup != bulleGroup) {
//			Debug.Log ("BulletScript2"+collider2D.gameObject.name); 
//			this.transform.GetComponent<BulletScript> ().enabled = false;
			isFinish = true;
			if (tweener!=null){
				tweener.Kill ();
				tweener = null;
			}
		}
	}

}
