using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[AddComponentMenu(AI/MiniCamera)]
public class MiniCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //获得屏幕分辨率
        float ratio = (float)Screen.width / (float)Screen.height;
        //使摄像机视图永远是一个正方形，rect的前两个参数表示XY位置，后两个表示XY大小
        //this.camera.rect = new Rect((1 - 0.25f), (1 - 0.2f * ratio), 0.2f, 2f * ratio);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
