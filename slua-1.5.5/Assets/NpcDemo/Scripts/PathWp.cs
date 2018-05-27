using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWp : MonoBehaviour {
    public Vector3[] wayPoints;//所有路点
    public int index = -1;//当前路点索引
    public Vector3 wayPoint;//当前路点
    bool isLoop = false;//是否循环
    public float deviation = 5;
    public bool isFinish = false;
    
    public bool isReach(Transform trans)//是否达到目的地
    {
        Vector3 pos = trans.position;
        float distance = Vector3.Distance(wayPoint, pos);
        return distance < deviation;
    }
    public void NextWayPoint()
    {
        if (index < 0) { return; }
        if(index<wayPoints.Length-1)
        {
            index++;
        }
        else
        {
            if (isLoop)
            {
                index = 0;
            }
            else
            {
                isFinish = true;
            }
        }
        wayPoint = wayPoints[index];
    }
    public void InitByObj(GameObject obj,bool isLoop =false)
    {
        int length = obj.transform.childCount;
        if(length == 0)
        {
            wayPoints = null;
            index = -1;
            Debug.LogWarning("Path.InitByObjlength == 0");
            return;

        }
        wayPoints = new Vector3[length];
        for(int i =0;i<length;i++)
        {
            Transform trans = obj.transform.GetChild(i);
            wayPoints[i] = trans.position;
        }
        index = 0;
        wayPoint = wayPoints[index];
        this.isLoop = isLoop;
        isFinish = false;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
