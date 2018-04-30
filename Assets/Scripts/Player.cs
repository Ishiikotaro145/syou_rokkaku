using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Vector3 moveVec = new Vector3();
	float moveSpeed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		moveVec.x = 0.0f;
		moveVec.y = 0.0f;
		if (Input.GetKey (KeyCode.UpArrow))
		{
			moveVec.y = moveSpeed;
		}
		if (Input.GetKey (KeyCode.DownArrow))
		{
			moveVec.y = -moveSpeed;
		}
		if (Input.GetKey (KeyCode.LeftArrow))
		{
			moveVec.x = -moveSpeed;
		}
		if (Input.GetKey (KeyCode.RightArrow))
		{
			moveVec.x = moveSpeed;
		}


		transform.position += moveVec;
	}
}
