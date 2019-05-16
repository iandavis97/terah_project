using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ORKFramework;
using ORKFramework.Behaviours;
using Invector.vCharacterController;

public class lockplayer : MonoBehaviour {
        private vThirdPersonInput m_input = null;	// Use this for initialization
	void Start () {
            m_input = GetComponent<vThirdPersonInput>();	}
	
	// Update is called once per frame
	void Update () {
        if (ORK.Control.InEvent || ORK.Control.InMenu || ORK.Control.Blocked)
        {
            m_input.lockInput = true;
        }
        else
        {
            m_input.lockInput = false;
        }  
	}
}
