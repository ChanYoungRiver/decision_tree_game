using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour {

	public LayerMask layer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GameObject dirObj = GameObject.Find ("Cube2");

		Ray ray = new Ray(this.transform.position,(dirObj.transform.position-this.transform.position  ).normalized);
		RaycastHit hit;
//		Physics.Raycast(ray,out hit);

		Physics.Raycast (ray,out hit,1000.0f,layer);
//		Physics.Raycast
		Debug.DrawLine(ray.origin,hit.point);


	}
}
