using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour {
    public Transform follow;
    public float distanceAway;
    public float distanceUp;
    public float smooth;
    private Vector3 targetPosition;
    private void LateUpdate()
    {
        targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(follow);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
