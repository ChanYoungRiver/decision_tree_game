using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	public SoliderPlayer m_Character;
	public TextAsset text;
	public BTree tree;

	public List<GameObject> linePointList;
	private Dictionary<int,GameObject> linePointMap;
	public int curPontIndex = 0;

	void Start () {
		m_Character = GetComponent<SoliderPlayer>();
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

	public GameObject GetCurPointObj(){
		GameObject obj;
		if (curPontIndex >= linePointList.Count){
			curPontIndex = 0;
		}
		linePointMap.TryGetValue (curPontIndex,out obj);
		return obj;

	}

	public void LoadTree(string str){
		BTreeMgr.sInstance.Load(text.text);
		this.tree = BTreeMgr.sInstance.GetTree(str);	
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(0,0,100,40),"load"))
		{
			LoadTree (text.name);
		}
	}
}
