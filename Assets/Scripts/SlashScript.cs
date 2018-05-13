using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine("Remove");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Remove()
	{
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}
}
