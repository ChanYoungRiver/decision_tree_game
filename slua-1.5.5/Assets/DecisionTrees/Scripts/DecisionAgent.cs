using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[AddComponentMenu("Decision Tree Toolkit/Decision Agent")]
public class DecisionAgent: Behaviour
{
	public DecisionTreeController decisionTree;

	void Awake()
	{	
		Hawk.AssertParameter(this, decisionTree, "decisionTree");
	}


	public bool? Decide(DecisionQuery query)
	{
		return decisionTree.Decide(query);
	}
}

