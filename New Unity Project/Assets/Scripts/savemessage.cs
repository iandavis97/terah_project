using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savemessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SendMessage("Awake");	
	
	}
	
	// Update is called once per frame
	void Update () {
	gameObject.SendMessage("RecordData");	
	}
}
