using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class loadmessage : MonoBehaviour {
public String s= "calyx";
	// Use this for initialization
	void Start () {
		gameObject.SendMessage("ApplyData", s);	
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
