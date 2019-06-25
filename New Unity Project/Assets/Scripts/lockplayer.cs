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
            ORK.InputKeys.Get(9).Blocked = true;//locking Tab key
        }
        else
        {
            m_input.lockInput = false;
            ORK.InputKeys.Get(9).Blocked = false;//unlocking Tab key
        }  
	}
}
