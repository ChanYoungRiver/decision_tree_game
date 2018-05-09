using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    //public static GameManager Instance = null;
    public int lifeValue = 100;//生命值
    public int ammoNum = 6;//弹药数量
    Player player;
    Text lab_ammoNum;
    Text lab_lifeValue;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lab_ammoNum = GameObject.Find("lab_ammoNum").GetComponent<Text>();
        lab_lifeValue = GameObject.Find("lab_lifeValue").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
      
    }
    //更新弹药
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
}
