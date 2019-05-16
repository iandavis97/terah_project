using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
using ORKFramework;
public class serialize2calyx : MonoBehaviour {
    public string calyx;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        	calyx= GetComponent<FactionMember>().SerializeToString();	
ORK.Game.Variables.Set("emotecalyx", calyx);

	}
}
