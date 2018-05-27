using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;

public class AIController : MonoBehaviour {

	public AIPlayer m_Character;
	public TextAsset text;//选树
	public BTree tree;//实例化树
	public static int TREE_INDEX = 0;//改名

	void Start () {
		m_Character = GetComponent<AIPlayer>();
	}
    //不断执行树
	// Update is called once per frame
	void Update () {
		if (this.tree != null && this.m_Character != null) {
			this.tree.Run (this.m_Character);
		}
	}
	//为每一个控制体加载树
	public void LoadTree(){
		BTreeMgr.sInstance.Load(text.text,TREE_INDEX);
		this.tree = BTreeMgr.sInstance.GetTree(text.name+TREE_INDEX);
		TREE_INDEX++;
	}

}
