using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ORKFramework;
using ORKFramework.Behaviours;
using Invector.vCharacterController;

public class lockplayer : MonoBehaviour {
    public vThirdPersonInput glorinput;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ORK.Control.InEvent || ORK.Control.InMenu || ORK.Control.Blocked)
        {
            glorinput.lockInput = true;
        }
        else
        {
            glorinput.lockInput = false;
        }  
	}
}
