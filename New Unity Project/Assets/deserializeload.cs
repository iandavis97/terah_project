using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
using System;
using ORKFramework;


public class deserializeload : MonoBehaviour {
	// Use this for initialization
    public String s;
	void Start () {
   	         s =ORK.Game.Variables.GetString("emote"+name.Replace("(clone)",""));
	GetComponent<FactionMember>().DeserializeFromString(s);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
