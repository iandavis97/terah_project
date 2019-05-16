using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
using System;
using ORKFramework;


public class deserializeloadcalyx : MonoBehaviour {
	// Use this for initialization
    public String s;
	void Start () {
        ORK.Game.Variables.Set("emotecalyx", 0);
   	         s =ORK.Game.Variables.GetString("emotecalyx");
	GetComponent<FactionMember>().DeserializeFromString(s);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
