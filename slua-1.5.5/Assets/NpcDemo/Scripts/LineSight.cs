using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSight : MonoBehaviour {
    public enum SightSensitivity { STRICT,LOOSE};    //我们视野的敏感度
    public SightSensitivity Sensitity = SightSensitivity.STRICT;//瞄准具灵敏度
    public bool CanSeeTarget = false;//能否看到敌人
    public float FieldOfView = 90f;//视野范围，士兵眼睛能看到的两侧之间的角度
    public Transform Target = null;//对目标的引用
    public Transform EyePoint = null;//对眼睛的引用
    private Transform ThisTransform = null;//对“transform”的引用
    private SphereCollider ThisCollider = null;//对球状碰撞题的引用
    public Vector3 LastKnowSighting = Vector3.zero;//对最后对象视野的引用
    private Collider[] SpottedEnemies; //附近的敌人


    private void Awake()
    {
        //获取组件
        ThisTransform = GetComponent<Transform>();
        ThisCollider = GetComponent<SphereCollider>();
        LastKnowSighting = ThisTransform.position;
        Target = GameObject.FindGameObjectWithTag("enemy").GetComponent<Transform>();
        //Target = GameObject.Find("Enemy").transform;

    }
    // Use this for initialization
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        Debug.Log(CanSeeTarget);
    }
    bool InFoV()
    {
         Vector3 DirToTarget = Target.position - EyePoint.position;//取得目标的方向
         float Angle = Vector3.Angle(EyePoint.forward, DirToTarget);//获取正前方和目标之间的角度
         if (Angle <= FieldOfView)
            return true; //确定敌人是否在视野中，但不考虑任何物理障碍   
         else
            return false;       
    }
    bool ClearLineofSight()//士兵的眼睛和敌人是否存在任何物理障碍
    {
        RaycastHit Info;
        if(Physics.Raycast(EyePoint.position,(Target.position - EyePoint.position).normalized,out Info, ThisCollider.radius)){
            if (Info.transform.CompareTag("enemy"))//如果看到敌人
                return true;
        }
        return false;
    }
    void UpdateSight()//与ClearLineofSight函数结合，  
    {
        switch(Sensitity)
        {
            case SightSensitivity.STRICT:
                CanSeeTarget = InFoV() && ClearLineofSight();
                break;
            case SightSensitivity.LOOSE:
                CanSeeTarget = InFoV() || ClearLineofSight();
                break;
        }
    }
    void OnTriggerStay(Collider other)//敌人进入该区域
    {
        UpdateSight();
        if (CanSeeTarget)//更新最后的视野
        {
            LastKnowSighting = Target.position;
        }
    }
    void OnTriggerExit(Collider other)//敌人离开该区域
    {
        if (!other.CompareTag("enemy")) return;
        CanSeeTarget = false;
    }



}
