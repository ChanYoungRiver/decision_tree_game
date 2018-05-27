using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;

public class EnemySoliderConV3 : PlayerController {


	void Start () {
		m_Character = GetComponent<SoliderPlayer>();
		Debug.Log ("m_Character"+m_Character+" text:"+text.name);
		LoadTree (text.name);

	}
		
	void Update () {
		if (this.tree != null && this.m_Character != null) {
			this.tree.Run (this.m_Character);
		}
	}

}
