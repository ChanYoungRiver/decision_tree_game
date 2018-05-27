using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSightV4 : MonoBehaviour {
    public enum SightSensitivity { STRICT,LOOSE};    //我们视野的敏感度
    public SightSensitivity Sensitity = SightSensitivity.STRICT;//瞄准具灵敏度
    public bool CanSeeTarget = false;//能否看到敌人
    public float FieldOfView = 180f;//视野范围，士兵眼睛能看到的两侧之间的角度
    private Transform Target = null;//对目标的引用
    public Transform EyePoint = null;//对眼睛的引用
    private Transform ThisTransform = null;//对“transform”的引用
    private SphereCollider ThisCollider = null;//对球状碰撞题的引用
    public Vector3 LastKnowSighting = Vector3.zero;//对最后对象视野的引用
	public AIPlayerGroup manGroup;
	public int solideIndex;

	public LayerMask layer;

	public GameObject enemyObj;

    private void Awake()
    {
        //获取组件
        ThisTransform = GetComponent<Transform>();
        ThisCollider = GetComponent<SphereCollider>();
        LastKnowSighting = ThisTransform.position;
        //Target = GameObject.FindGameObjectWithTag("enemy").GetComponent<Transform>();
    }
    bool InFoV()
    {
		//GameLauncher5.GetInstance().aiPlayerList
		Debug.LogError ("Athis.manGroup"+this.manGroup+" "+solideIndex);
		foreach (GameObject enemy in GameLauncher5.GetInstance().aiPlayerList) 
		{
			AI3DPlayer aIPlayer = enemy.GetComponent<AI3DPlayer>();
			Debug.LogError ("this.manGroup@@"+this.manGroup+" "+solideIndex+" "+aIPlayer.SOLIDER_INDEX+" "+aIPlayer.belongGroup);
			if (solideIndex!=aIPlayer.SOLIDER_INDEX &&aIPlayer.belongGroup != manGroup) 
			{
				Debug.LogError ("this.manGroup"+this.manGroup+" "+solideIndex);
				Target = enemy.transform;
				Vector3 DirToTarget = Target.position - EyePoint.position;//取得目标的方向
				float Angle = Vector3.Angle(EyePoint.forward, DirToTarget);//获取正前方和目标之间的角度

				//EyePoint.right
				if (Angle <= FieldOfView)
				{ return true; }//确定敌人是否在视野中，但不考虑任何物理障碍  
			}


		}
		Target = null;
        return false;
    }
    bool ClearLineofSight()//士兵的眼睛和敌人是否存在任何物理障碍
    {

		foreach (GameObject enemy in GameLauncher5.GetInstance().aiPlayerList) 
		{
			AI3DPlayer aIPlayer = enemy.GetComponent<AI3DPlayer>();



			RaycastHit Info;
//			Target = enemy.transform;
			Transform[] rect = enemy.transform.GetComponentsInChildren<Transform> ();
			foreach(Transform ts in rect)
			{
				if (ts.name == "EyePoint") {
					Target = ts;
				}
			}
			if(solideIndex != aIPlayer.SOLIDER_INDEX&&
				aIPlayer.belongGroup != manGroup &&
				Physics.Raycast(EyePoint.position,(Target.position-EyePoint.position).normalized,out Info, ThisCollider.radius,layer  )){

				Ray ray = new Ray(EyePoint.position,(Target.position-EyePoint.position).normalized);
				RaycastHit hit;
				Physics.Raycast (ray,out hit,1000.0f,layer);
				Debug.DrawLine(ray.origin,hit.point,Color.red);
//				Debug.DrawLine (Target.position,(EyePoint.position-Target.position).normalized,Color.red);
//				Debug.DrawLine (EyePoint.position,(Target.position-EyePoint.position).normalized,Color.yellow);
//
//				Debug.DrawLine (EyePoint.position,Target.position,Color.green);
//				if (Info.transform.CompareTag("Player") )
				if (Info.collider.transform.tag.Equals("EnemyPhy"))
				{
					if (manGroup == AIPlayerGroup.A) {
						GameObject.Find("lab_state3").GetComponent<Text>().text = manGroup+" "+aIPlayer.SOLIDER_INDEX;
					}else{
						GameObject.Find("lab_state4").GetComponent<Text>().text = manGroup+" "+aIPlayer.SOLIDER_INDEX;
					}
				
						//Debug.LogError ("如果看到敌人");
					Debug.LogError ("如果看到敌人:"+manGroup+" "+Info.transform.gameObject.name);
						enemyObj = Info.transform.gameObject;
						return true;


				}//如果看到敌人

			}
		}
		Target = null;
        return false;
    }
    void UpdateSight()//与ClearLineofSight函数结合，判断敌人是否在无遮挡的视野范围內
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
		

		if (other.transform.CompareTag ("EnemyPhy")&&other.transform.parent!=this.transform) {
			UpdateSight();

			if (CanSeeTarget)//更新最后的视野
			{
				LastKnowSighting = Target.position;
				Debug.LogError ("OnTriggerStay:"+other.gameObject.name);
				enemyObj = other.gameObject;
			}
		}
  
    }
    void OnTriggerExit(Collider other)//敌人离开该区域
    {
//		if (!other.CompareTag("Player")) return;
//        CanSeeTarget = false;
//		enemyObj = null;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (CanSeeTarget) {
			Debug.LogError ("CanSeeTarget"+CanSeeTarget);
		}

	}
}
