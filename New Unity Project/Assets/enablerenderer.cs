using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class enablerenderer : MonoBehaviour {

	// Use this for initialization
    void OnEnable(){	

    }
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		        Lua.RegisterFunction("enablerender", this, typeof(enablerenderer).GetMethod("enablerender"));
    Lua.RegisterFunction("disablerender", this, typeof(enablerenderer).GetMethod("disablerender"));
	}
    
    public void enablerender(){
         foreach (Renderer r in transform.GetComponentsInChildren<Renderer>(true))
   r.enabled = true;

    }
       public void disablerender(){
         foreach (Renderer r in transform.GetComponentsInChildren<Renderer>(true))
   r.enabled = false;

    }
}
