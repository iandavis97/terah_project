using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ORKFramework;
using ORKFramework.Behaviours;
using Invector.vCharacterController;

public class lockplayer : MonoBehaviour
{
        private vThirdPersonInput m_input = null;
    private vThirdPersonController m_move = null;
    // Use this for initialization
	void Start ()
    {
            m_input = GetComponent<vThirdPersonInput>();
            m_move = GetComponent<vThirdPersonController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ORK.Control.InEvent || ORK.Control.InMenu || ORK.Control.Blocked || ORK.Game.Variables.Check("convoFlag", true))
        {
            m_input.lockInput = true;
        }
        else if(ORK.Game.Variables.Check("convoFlag", false))
            m_input.lockInput = false;
        else
        {
            m_input.lockInput = false;
        }  
	}
}
