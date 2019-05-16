using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
using ORKFramework;
public class serializeload : MonoBehaviour {
    public string calyx;
	// Use this for initialization
	void Start () {
                	calyx= GetComponent<FactionMember>().SerializeToString();	
ORK.Game.Variables.Set("emotecalyx", calyx);
	}
	
	// Update is called once per frame
	void Update () {


	}
}
