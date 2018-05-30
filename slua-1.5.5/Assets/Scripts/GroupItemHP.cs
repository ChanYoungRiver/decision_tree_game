using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupItemHP : MonoBehaviour {

	// Use this for initialization
	public AI3DPlayer player;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

//		Transform labObj = this.transform.Find ("lab_hp");
//		labObj.GetComponent<Text> ().text = "HP:"+hp;
//		Transform labObj = this.transform.Find ("img_hp");
//		labObj.GetComponent<Image> ().fillAmount = "HP:"+hp;
		updateHp();

	}

	public void updateHp(){

		float value = (float)player.hp / 100f;
		Transform imgHp = this.transform.Find ("img_hp");
		imgHp.GetComponent<Image> ().fillAmount = value;

		Transform labHp = this.transform.Find ("lab_hp");
		labHp.GetComponent<Text> ().text = ""+player.hp;

		Transform lab_state = this.transform.Find ("lab_state");
		lab_state.GetComponent<Text> ().text = ""+player.manState;

		Transform labBullet = this.transform.Find ("lab_bullet_num");
		labBullet.GetComponent<Text> ().text = ""+player.ammoNum;


	}

}
