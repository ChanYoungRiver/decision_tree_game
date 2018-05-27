using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3DPlayer : SoliderPlayer
{

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {

        Transform labObj = this.transform.Find("lab_hp");
        labObj.GetComponent<TextMesh>().text = "HP:" + hp;

    }

    void OnTriggerEnter(Collider collider3D)
    {
        //		Debug.Log ("BulletScript1"+collider2D.gameObject.name); 
        BulletScript bullet = collider3D.GetComponent<BulletScript>();
        if (bullet != null)
        {
            //			Debug.Log ("bullet"+collider2D.gameObject.name); 
            hp = hp - bullet.damage;
            if (hp <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
