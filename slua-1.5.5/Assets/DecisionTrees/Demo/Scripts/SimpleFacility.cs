using System;
using UnityEngine;


// Simple behavior class that will simulate to satisfy the need of the NPC
[AddComponentMenu("AI/Demo/Simple Facility")]
public class SimpleFacility: MonoBehaviour
{
	public string inputName;
	public DemoUI demoUI;

	void OnTriggerEnter(Collider other)
	{
		demoUI.SatisfyNeed(inputName);
		demoUI.Decide();
	}
}


