using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using System;
using System.Text;

[AddComponentMenu("AI/Demo/Demo UI")]
public class DemoUI : MonoBehaviour {

	[Serializable]
	public class NeedSelector {
		public string inputName;
		public Toggle toggle;
		public Transform target;
	}

	public DecisionTreeController tree;
	public Text text;
	public Text treePrint;
	public Button execute;
	public Dropdown treeSelect;
	public AICharacterControl character;
	public NeedSelector[] needs;
	public float near = 10f;
	public string treeName = "InstinctSelector";
	public Transform startPoint;

	// Use this for initialization
	void Start () {
		treeSelect.options.Clear();
		treeSelect.onValueChanged.AddListener(OnTreeSelect);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Decide()
	{
		// Calculate the sqr to compare to the dot product 
		float nearSqr = near * near;
		List<AttributeValue> values = new List<AttributeValue>();
		foreach(var need in needs)
		{
			var dist = need.target.transform.position - character.transform.position;

			// If the need is selected we simple state whether its Near or Far
			// If it isn't selected we simply state "No"
			values.Add(new AttributeValue(need.inputName, 
				need.toggle.isOn 
					? ((dist.sqrMagnitude > nearSqr) ? "Far" : "Near")
					: "No"));
		}

		// Now we use the decision tree to see which of the needs we need to satisfy first
		// o In human reasoning thirst and hunger will always take precedence over other needs
		// o The distance to satisfy the need is also a factor
		// o Please note that in a game context these needs would have their own
		//   mecanisms such as using energy depletion as an indication of hunger.

		DecisionQuery query = new DecisionQuery();

		bool decided = false;
		StringBuilder builder = new StringBuilder();
		foreach(var need in needs)
		{
			if (!need.toggle.isOn)
				continue;

			query.Set(treeName, need.inputName, values.ToArray());

			tree.Decide(query);
			if (query.Yes) {
				character.target = need.target;
				decided = true;
			}
			builder.AppendLine(tree.PrintDecision(query));
		}
		// Print the last query
		text.text = builder.ToString();
		if (!decided)
			character.target = startPoint;
	}

	public void OnExecute()
	{
		Decide();
	}


	public void OnDecisionTreeReady(TreeNode root)
	{
		//text.text = root.PrintNode("--->");
		treeSelect.options.Add(new Dropdown.OptionData(root.attribute.CustomAttributeName));
		treeSelect.RefreshShownValue();
		if (treeSelect.options.Count == 1)
		{
			OnTreeSelect(0);
		}
	}

	public void OnTreeSelect(int index)
	{
		var opt = treeSelect.options[index];
		var info = tree.GetDecisionTree(treeName);
		var outcome = info.GetOutcome(opt.text);
		this.treePrint.text = outcome.root.PrintNode();
	}

	public void SatisfyNeed(string inputName)
	{
		for(int i = 0; i < this.needs.Length; i++)
		{
			if (needs[i].inputName == inputName)
			{
				needs[i].toggle.isOn = false;
			}
		}
	}


}
