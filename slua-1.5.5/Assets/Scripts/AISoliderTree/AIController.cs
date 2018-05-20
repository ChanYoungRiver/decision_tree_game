using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;

public class AIController : MonoBehaviour {

	public AIPlayer m_Character;
	public TextAsset text;
	public BTree tree;

	public static int TREE_INDEX = 0;

	public List<GameObject> linePointList;
	private Dictionary<int,GameObject> linePointMap;
	public int curPontIndex = 0;

	void Start () {
		m_Character = GetComponent<AIPlayer>();
		linePointMap = new Dictionary<int,GameObject> ();
		int i = 0;
		foreach(GameObject obj in linePointList){
			linePointMap.Add (i++,obj);
		}
	}

	// Update is called once per frame
	void Update () {
		if (this.tree != null && this.m_Character != null) {
			this.tree.Run (this.m_Character);
		}
	}
		
	public void LoadTree(){
		BTreeMgr.sInstance.Load(text.text,TREE_INDEX);
		this.tree = BTreeMgr.sInstance.GetTree(text.name+TREE_INDEX);
		TREE_INDEX++;
	}

//	void OnGUI()
//	{
//		if(GUI.Button(new Rect(0,0,100,40),"load"))
//		{
//			LoadTree (text.name);
//		}
//	}

}
