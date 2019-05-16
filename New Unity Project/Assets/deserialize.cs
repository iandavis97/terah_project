using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
using System;
using ORKFramework;


public class deserialize : MonoBehaviour {
	// Use this for initialization
    public String s;
	void Start () {
   	
	}
	
	// Update is called once per frame
	void Update () {
         s =ORK.Game.Variables.GetString("emotecalyx");
	GetComponent<FactionMember>().DeserializeFromString(s);
	}
}
